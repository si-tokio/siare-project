using siare.Server.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace siare.Test.Server.Helpers
{
  [TestClass]
  public class EncryptHelperTest
  {
    [TestMethod]
    public void TestEncryptDecrypt()
    {
      // Arrange
      string originalText = "Hello, World!";

      // Act
      string encryptedText = EncryptHelper.Encrypt(originalText);
      string decryptedText = EncryptHelper.Decrypt(encryptedText);

      // Assert
      Assert.AreNotEqual(originalText, encryptedText, "Encrypted text should not be the same as the original text.");
      Assert.AreEqual(originalText, decryptedText, "Decrypted text should be the same as the original text.");
    }

    [TestMethod]
    public void TestDecryptWithWrongCipher()
    {
      // Arrange
      string wrongCipherText = Convert.ToBase64String(Encoding.UTF8.GetBytes("wrong-cipher-text"));

      // Act and Assert
      Assert.ThrowsException<CryptographicException>(() => EncryptHelper.Decrypt(wrongCipherText), "Decrypting with a wrong cipher text should throw a CryptographicException.");
    }

  }
}
