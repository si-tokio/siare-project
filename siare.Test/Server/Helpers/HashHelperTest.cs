using siare.Server.Helpers;

namespace siare.Test.Server.Helpers
{
  [TestClass]
  public class HashHelperTest
  {
    [TestMethod]
    public void TestGenerateSalt()
    {
      string salt = HashHelper.GenerateSalt();

      Assert.IsNotNull(salt);
      Assert.IsTrue(salt.Length > 0);
    }

    [TestMethod]
    public void TestHashPassword()
    {
      string password = "testPassword";
      string salt = HashHelper.GenerateSalt();

      string hashedPassword = HashHelper.HashPassword(password, salt);

      Assert.IsNotNull(hashedPassword);
      Assert.IsTrue(hashedPassword.Length > 0);
    }

    [TestMethod]
    public void TestHashPasswordWithSameSalt()
    {
      string password1 = "testPassword";
      string password2 = "testPassword";
      string salt = HashHelper.GenerateSalt();

      string hashedPassword1 = HashHelper.HashPassword(password1, salt);
      string hashedPassword2 = HashHelper.HashPassword(password2, salt);

      Assert.AreEqual(hashedPassword1, hashedPassword2);
    }
  }
}
