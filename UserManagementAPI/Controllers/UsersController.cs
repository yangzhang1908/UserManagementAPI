using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        /* private static List<User> _users = new List<User>
         {
             new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Department = "HR" },
             new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Department = "IT" }
         };*/

        // Change from List to Dictionary for O(1) lookups by Id
        private static Dictionary<int, User> _users = new Dictionary<int, User>
        {
            { 1, new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Department = "HR" } },
            { 2, new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Department = "IT" } }
        };


        // ... (other CRUD methods will be added below)

        // GET: api/users
        [HttpGet]
        public IActionResult GetUsers()
        {
            // This is where you would normally retrieve users from a database
            return Ok(new List<string> { "User1", "User2", "User3" });
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                /*                var user = _users.FirstOrDefault(u => u.Id == id);
                                if (user == null)
                                {
                                    return NotFound($"User with ID {id} not found."); // Enhanced message
                                }
                                return Ok(user);*/
                if (_users.TryGetValue(id, out var user)) // O(1) lookup
                {
                    return Ok(user);
                }
                return NotFound($"User with ID {id} not found.");
            }
            catch (Exception ex)
            {
                // In a real app, log the exception (e.g., using ILogger)
                Console.WriteLine($"Error retrieving user: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            // Copilot will likely suggest this if ModelState.IsValid is not already checked
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            if (user.Id == 0)
            {
                user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            }
            _users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Simple auto-increment for in-memory store, ensuring unique ID
            user.Id = _users.Any() ? _users.Keys.Max() + 1 : 1;
            _users.Add(user.Id, user); // Add to dictionary with ID as key
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // ...

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            /*if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            // Copilot will suggest this
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns validation errors
            }

            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Department = user.Department;

            return NoContent();*/
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_users.TryGetValue(id, out var existingUser))
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Department = user.Department;
                return NoContent();
            }
            return NotFound($"User with ID {id} not found.");
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            /* var user = _users.FirstOrDefault(u => u.Id == id);
             if (user == null)
             {
                 return NotFound();
             }

             _users.Remove(user);
             return NoContent(); // 204 No Content for successful deletion*/

            if (_users.ContainsKey(id))
            {
                _users.Remove(id);
                return NoContent();
            }
            return NotFound($"User with ID {id} not found.");
        }

    }
}
