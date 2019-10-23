using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Controllers
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserContext context;

        public UserController(UserContext context)
        {
            this.context = context;

            if (this.context.Users.Count() == 0)
            {
                User auxUser = new User()
                {
                    Id = 1,
                    Name = "Alberto",
                    LastName = "Melero",
                    Address = "Home",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                this.context.Users.Add(auxUser);
                this.context.SaveChanges();
            }
        }

        #region CRUD methods

        /// <summary>
        /// GET users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }

        /// <summary>
        /// GET user by id (api/user/id)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int userID)
        {
            User user = await context.Users.FindAsync(userID);

            if (user == null)
                return NotFound();

            return user;
        }

        /// <summary>
        /// POST: api/user 
        /// </summary>
        /// <param name="user">User to post</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostTodoItem(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// DELETE user by id
        /// </summary>
        /// <param name="userID">ID of the selected user</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int userID)
        {
            User user = await context.Users.FindAsync(userID);

            if (user == null)
                return NotFound();

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        #endregion
    }
}
