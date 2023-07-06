using siare.Shared.Entities;

namespace siare.Server.Repositories.Oracle.Users
{
  /// <summary>
  /// IUserRepository
  /// </summary>
  public interface IUserRepository
  {
    /// <summary>
    /// すべてのユーザーを非同期に取得します。
    /// </summary>
    /// <returns>非同期操作を表すタスク。タスクの結果はユーザーのリストを含みます。</returns>
    Task<IEnumerable<User>> GetUsersAsync();

    /// <summary>
    /// 指定したIDのユーザーを非同期に取得します。
    /// </summary>
    /// <param name="id">ユーザーのID。</param>
    /// <returns>非同期操作を表すタスク。タスクの結果は見つかった場合はユーザー、そうでない場合はnullを含みます。</returns>
    Task<User> GetUserByIdAsync(int id);

    /// <summary>
    /// 指定したIDとPasswordのユーザーを非同期に取得します。
    /// </summary>
    /// <param name="id">ユーザーのID。</param>
    /// <param name="passwordHash">ユーザーのパスワードハッシュ。</param>
    /// <returns>非同期操作を表すタスク。タスクの結果は見つかった場合はユーザー、そうでない場合はnullを含みます。</returns>
    Task<User> GetUserByIdAndPasswordAsync(int id, string passwordHash);

    /// <summary>
    /// 新たなユーザーを非同期に作成します。
    /// </summary>
    /// <param name="user">作成するユーザー。</param>
    /// <returns>非同期操作を表すタスク。タスクの結果は作成されたユーザーを含みます。</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// 既存のユーザーを非同期に更新します。
    /// </summary>
    /// <param name="user">更新するユーザー。</param>
    /// <returns>非同期操作を表すタスク。タスクの結果は更新されたユーザーを含みます。</returns>
    Task<User> UpdateUserAsync(User user);

    /// <summary>
    /// 指定したIDのユーザーを非同期に削除します。
    /// </summary>
    /// <param name="id">削除するユーザーのID。</param>
    /// <returns>非同期操作を表すタスク。</returns>
    Task<int> DeleteUserAsync(int id);
  }
}
