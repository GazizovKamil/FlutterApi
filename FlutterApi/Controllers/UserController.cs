using FlutterApi.Data;
using FlutterApi.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Objects;

namespace FlutterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly FlutterApiDB dbContext;

        public UserController(FlutterApiDB dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            //var a = new {Guid Id, string? Name, string? Email};
            var user = dbContext.Users.Select(p => new
            {
                Id = p.id,
                Name = p.name,
                Email = p.email
            });
            
            return Ok(user);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetUser([FromRoute] Guid id)
        {
            var user = dbContext.Users.Select(p => new
            {
                Id = p.id,
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
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            var user = new User()
            {
                id = Guid.NewGuid(),
                name = addUserRequest.name,
                email = addUserRequest.email,
                password = addUserRequest.password,
                CreatedDate = DateTime.Now,
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UpdateUserRequest updateUserRequest)
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

                return Ok(GetUser(id));
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("Delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if(user != null)
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("GetUsersGmail")]
        public IActionResult GetUsersGmails()
        {
            //var a = new {Guid Id, string? Name, string? Email};
            var user = dbContext.Users.Select(p => new
            {
                Id = p.id,
                Name = p.name,
                Email = p.email
            }).Where(z=> z.Email.Contains("@mail.ru"));

            return Ok(user);
        }

        [HttpGet]
        [Route("GetUsersArsenii")]
        public IActionResult GetUsersAsrenii()
        {
            //var a = new {Guid Id, string? Name, string? Email};
            var user = dbContext.Users.Select(p => new
            {
                Id = p.id,
                Name = p.name,
                Email = p.email
            }).Where(z => z.Name == "Арсений");

            return Ok(user);
        }
    }
}
