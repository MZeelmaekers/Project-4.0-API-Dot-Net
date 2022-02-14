using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Project40_API_Dot_NET.Data;
using Project40_API_Dot_NET.Models;
using Project40_API_Dot_NET.Services;

namespace Project40_API_Dot_NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PlantContext _context;

        private IUserService _userService;

        public UserController(PlantContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        // GET: api/User/Supervisors
        [HttpGet("SuperVisors")]
        public async Task<ActionResult<IEnumerable<User>>> GetSuperVisors()
        {
            List<User> users = await _context.Users
                .Where(u => u.Role == Role.SuperVisor || u.Role == Role.Admin)
                .Include(u => u.CameraBoxes)
                .Include(u => u.Plants)
                .ToListAsync();

            // Prevent from sending the password
            users.ForEach(u => u.Password = null);

            return users;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            List<User> users = await _context.Users
                .Include(u => u.CameraBoxes)
                .Include(u => u.Plants)
                .ToListAsync();

            // Prevent from sending the password
            users.ForEach(u => u.Password = null);

            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.CameraBoxes)
                .Include(u => u.Plants)
                .Where(u => u.Id == id)
                .FirstAsync();

            if (user == null)
            {
                return NotFound();
            }

            user.Password = null;

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            // Prevent users from changing their passwords via this route
            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(user).Property(u => u.Password).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/User/password/{id}
        [Authorize]
        [HttpPut("password/{id}")]
        public async Task<IActionResult> UserChangePassword(int id,[FromBody] JObject fromBody)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }

            var jobject = JObject.Parse(fromBody.ToString());
            string password = (string)jobject.SelectToken("password");
            string newPassword = (string)jobject.SelectToken("newPassword");
            User user = await _context.Users.FindAsync(id);
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return BadRequest();
            }

            string hashed = BCrypt.Net.BCrypt.HashPassword(newPassword, 13);
            user.Password = hashed;
            _context.Entry(user).Property(u => u.Password).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Check if an user exists with the given email
            var userExists = _context.Users
               .Any(u => u.Email == user.Email);
            if (userExists)
            {
                throw new ArgumentException();
            }
            string hashed = BCrypt.Net.BCrypt.HashPassword(user.Password, 13);

            user.Password = hashed;

            // A new user is standard a User
            user.Role = Role.User;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            // Make sure there is still at least 1 admin.
            if (isNotAdmin(id))
            {
                if (!AdminExists())
                {
                    throw new AccessViolationException();
                }
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool AdminExists()
        {
            int count = _context.Users
                .Where(e => e.Role == Role.Admin)
                .Count();
            if (count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isNotAdmin(int id)
        {
            return _context.Users
                .Where(e => e.Role == Role.Admin)
                .Any(e => e.Id == id);
        }
    }
}
