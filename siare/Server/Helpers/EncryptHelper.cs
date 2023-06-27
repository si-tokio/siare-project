using System.Security.Cryptography;
using System.Text;

namespace siare.Server.Helpers
{
  /// <summary>
  /// Encrypt Helper
  /// </summary>
  public static class EncryptHelper
  {
    private static byte[] _key = Encoding.UTF8.GetBytes("1234567890123456"); // シークレットキー (16バイト)
    private static byte[] _iv = Encoding.UTF8.GetBytes("abcdefghijklmnop"); // 初期化ベクトル (16バイト)

    /// <summary>
    /// 与えられた平文の文字列を暗号化します。
    /// </summary>
    /// <param name="plainText">暗号化する平文の文字列。</param>
    /// <returns>暗号化された文字列。</returns>
    public static string Encrypt(string plainText)
    {
      byte[] encrypted;

      using (Aes aes = Aes.Create())
      {
        aes.Key = _key;
        aes.IV = _iv;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(plainText);
            }

            encrypted = ms.ToArray();
          }
        }
      }

      return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 与えられた暗号化された文字列を復号化します。
    /// </summary>
    /// <param name="cipherText">復号化する暗号化された文字列。</param>
    /// <returns>元の平文の文字列。</returns>
    public static string Decrypt(string cipherText)
    {
      string decrypted;

      using (Aes aes = Aes.Create())
      {
        aes.Key = _key;
        aes.IV = _iv;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
        {
          using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader sr = new StreamReader(cs))
            {
              decrypted = sr.ReadToEnd();
            }
          }
        }
      }

      return decrypted;
    }
  }
}
