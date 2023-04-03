using FlutterApi.Data;
using FlutterApi.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await dbContext.Users.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);

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

            return Ok(user);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UpdateUserRequest updateUserRequest)
        {
            var user = dbContext.Users.Find(id);

            if(user != null)
            {
                user.name = updateUserRequest.name;
                user.email = updateUserRequest.email;
                user.password = updateUserRequest.password;
                user.UpdateTime = DateTime.Now;

                await dbContext.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
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
    }
}
