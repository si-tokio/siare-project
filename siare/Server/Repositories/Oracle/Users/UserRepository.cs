using siare.Server.Helpers;
using siare.Shared.Entities;

namespace siare.Server.Repositories.Oracle.Users
{
  /// <summary>
  /// User Repository
  /// </summary>
  public class UserRepository : IUserRepository
  {
    public async Task<User> CreateUserAsync(User user)
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
      await OracleHelper.ExecuteAsync(sql, user);
      return user;
    }

    public async Task<int> DeleteUserAsync(int id)
    {
      var result = await OracleHelper.ExecuteAsync($"DELETE FROM users WHERE user_id = :{nameof(id)}", new { id });
      return result;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
      var user = await OracleHelper.GetOneAsync<User, object>($@"SELECT user_id, username, password_hash, salt, email FROM users WHERE user_id = :{nameof(id)}", new { id });
      return user;
    }

    public async Task<User> GetUserByIdAndPasswordAsync(int id, string passwordHash)
    {
      var user = await OracleHelper.GetOneAsync<User, object>($@"SELECT user_id, username, password_hash, salt, email FROM users WHERE user_id = :{nameof(id)} AND password_hash = :{nameof(passwordHash)}", new { id, passwordHash });
      return user;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
      var users = await OracleHelper.GetAsync<User>("SELECT user_id, username, password_hash, salt, email FROM users ORDER BY user_id");
      return users;
    }

    public async Task<User> UpdateUserAsync(User user)
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
      await OracleHelper.ExecuteAsync(sql, user);
      return user;
    }
  }
}
