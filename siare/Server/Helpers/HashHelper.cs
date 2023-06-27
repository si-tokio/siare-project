using System.Security.Cryptography;
using System.Text;

namespace siare.Server.Helpers
{
  /// <summary>
  /// Hash Helper
  /// </summary>
  public static class HashHelper
  {
    /// <summary>
    /// 新たなソルトを生成するメソッドです。
    /// 生成されたソルトは、パスワードをハッシュ化する際に使用します。
    /// </summary>
    /// <returns>生成されたソルト</returns>
    public static string GenerateSalt()
    {
      byte[] bytes = new byte[128 / 8];
      using (var keyGenerator = RandomNumberGenerator.Create())
      {
        keyGenerator.GetBytes(bytes);
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
      }
    }

    /// <summary>
    /// ソルトと一緒にパスワードをハッシュ化するメソッドです。
    /// パスワードとソルトを連結し、その結果をハッシュ化します。
    /// </summary>
    /// <param name="password">ハッシュ化するパスワード</param>
    /// <param name="salt">ハッシュ化に使用するソルト</param>
    /// <returns>ハッシュ化されたパスワード</returns>
    public static string HashPassword(string password, string salt)
    {
      using (var sha256 = SHA256.Create())
      {
        var saltedPassword = password + salt;
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
      }
    }
  }
}
