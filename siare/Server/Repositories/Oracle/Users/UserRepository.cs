using siare.Server.Helpers;
using siare.Shared.Entities;

namespace siare.Server.Repositories.Oracle.Users
{
  /// <summary>
  /// User Repository
  /// </summary>
  public class UserRepository : IUserRepository
  {
    public Task<User> CreateUserAsync(User user)
    {
      var sql = $@"
INSERT INTO users(
  user_id,
  username,
  password_hash,
  email
)
VALUES(
  :{nameof(user.UserId)},
  :{nameof(user.Username)},
  :{nameof(user.PasswordHash)},
  :{nameof(user.Email)}
)
";
      OracleHelper.Execute(sql, user);
      return Task.FromResult(user);
    }

    public Task DeleteUserAsync(int id)
    {
      var result = OracleHelper.Execute($"DELETE FROM users WHERE userid = :{id}", id);
      return Task.FromResult(result);
    }

    public Task<User> GetUserByIdAsync(int id)
    {
      var user = OracleHelper.GetOne<User>($@"SELECT * FROM users WHERE userid = :{id}");
      return Task.FromResult<User>(user);
    }

    public Task<IEnumerable<User>> GetUsersAsync()
    {
      var users = OracleHelper.Get<User>("SELECT * FROM users");
      return Task.FromResult<IEnumerable<User>>(users);
    }

    public Task<User> UpdateUserAsync(User user)
    {
      var sql = $@"
UPDATE
  users
SET
  username = :{nameof(user.Username)},
  passwordhash = :{nameof(user.PasswordHash)},
  email = :{nameof(user.Email)}
WHERE
  userid = :{nameof(user.UserId)}
";
      OracleHelper.Execute(sql, user);
      return Task.FromResult(user);
    }
  }
}
