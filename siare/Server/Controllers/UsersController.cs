using Microsoft.AspNetCore.Mvc;
using siare.Shared.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace siare.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private static readonly List<User> Users = new List<User>
    {
        new User { UserId = 1, Username = "User1", PasswordHash = "password1", Email = "user1@example.com" },
        new User { UserId = 2, Username = "User2", PasswordHash = "password2", Email = "user2@example.com" },
        // Add more users if needed
    };

    // GET: api/Users
    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
      return Ok(Users);
    }

    // GET api/Users/5
    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
      var user = Users.FirstOrDefault(u => u.UserId == id);

      if (user == null)
      {
        return NotFound();
      }

      return Ok(user);
    }

    // POST api/<UsersController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
