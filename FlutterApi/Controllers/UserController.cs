using FlutterApi.Data;
using FlutterApi.Hubs;
using FlutterApi.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;

namespace FlutterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly FlutterApiDB dbContext;
        private readonly IHubContext<SignalrHub> _hubContext;

        public UserController(FlutterApiDB dbContext, IHubContext<SignalrHub> hubContext)
        {
            this.dbContext = dbContext;
            this._hubContext = hubContext;
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            //var a = new {Guid Id, string? Name, string? Email};
            var user = dbContext.Users.Select(p => new
            {
                Id = p.Id,
                Name = p.name,
                Email = p.email
            });
            
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUser/{id:int}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            var user = dbContext.Users.Select(p => new
            {
                Id = p.Id,
                Name = p.name,
                Email = p.email
            }).Where(z => z.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            var user = new User()
            {
                name = addUserRequest.name,
                email = addUserRequest.email,
                password = addUserRequest.password,
                CreatedDate = DateTime.Now,
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("User", user);

            return Ok("Пользователь добавлен!");
        }

        [HttpPut]
        [Route("Update/{id:int}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, UpdateUserRequest updateUserRequest)
        {
            var user = dbContext.Users.Find(id);

            if (updateUserRequest.name != null) user.name = updateUserRequest.name;
            if (updateUserRequest.email != null) user.email = updateUserRequest.email;

            if (user != null)
            {
                user.name = user.name;
                user.email = user.email;
                user.UpdateTime = DateTime.Now;

                await dbContext.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("User", user);

                return Ok(GetUser(id));
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if(user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("User", user);

                return Ok("Пользователь удален!");
            }

            return NotFound();
        }
    }
}
