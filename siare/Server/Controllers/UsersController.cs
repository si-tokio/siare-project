using Microsoft.AspNetCore.Mvc;
using siare.Server.Helpers;
using siare.Server.Repositories.Oracle.Users;
using siare.Shared.Entities;

namespace siare.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
    {
      _logger = logger;
      _userRepository = userRepository;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
      try
      {
        var users = await _userRepository.GetUsersAsync();
        return Ok(users);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

    // GET api/Users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
      try
      {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
          return NotFound();
        }

        return Ok(user);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

    // POST api/<UsersController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User user)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        SetHashPassword(user);
        var addedUser = await _userRepository.CreateUserAsync(user);
        return CreatedAtAction(nameof(Get), new { id = addedUser.UserId }, addedUser);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

    private static void SetHashPassword(User user)
    {
      user.Salt = HashHelper.GenerateSalt();
      user.PasswordHash = HashHelper.HashPassword(user.PasswordHash, user.Salt);
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] User user)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return BadRequest(ModelState);
        }
        var updatedUser = await _userRepository.UpdateUserAsync(user);
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      try
      {
        var deleted = await _userRepository.DeleteUserAsync(id);
        return NoContent();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }
  }
}
