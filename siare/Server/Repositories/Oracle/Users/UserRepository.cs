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
  salt,
  email
)
VALUES(
  :{nameof(user.UserId)},
  :{nameof(user.Username)},
  :{nameof(user.PasswordHash)},
  :{nameof(user.Salt)},
  :{nameof(user.Email)}
)
";
      OracleHelper.Execute(sql, user);
      return Task.FromResult(user);
    }

    public Task<int> DeleteUserAsync(int id)
    {
      var result = OracleHelper.Execute($"DELETE FROM users WHERE user_id = :{nameof(id)}", new { id });
      return Task.FromResult(result);
    }

    public Task<User> GetUserByIdAsync(int id)
    {
      var user = OracleHelper.GetOne<User, object>($@"SELECT user_id, username, password_hash, salt, email FROM users WHERE user_id = :{nameof(id)}", new { id });
      return Task.FromResult<User>(user);
    }

    public Task<IEnumerable<User>> GetUsersAsync()
    {
      var users = OracleHelper.Get<User>("SELECT user_id, username, password_hash, salt, email FROM users ORDER BY user_id");
      return Task.FromResult<IEnumerable<User>>(users);
    }

    public Task<User> UpdateUserAsync(User user)
    {
      var sql = $@"
UPDATE
  users
SET
  username = :{nameof(user.Username)},
  password_hash = :{nameof(user.PasswordHash)},
  salt = :{nameof(user.Salt)},
  email = :{nameof(user.Email)}
WHERE
  user_id = :{nameof(user.UserId)}
";
      OracleHelper.Execute(sql, user);
      return Task.FromResult(user);
    }
  }
}
