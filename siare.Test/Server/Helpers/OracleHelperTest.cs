using siare.Server.Helpers;
using siare.Shared.Entities;

namespace siare.Test.Server.Helpers
{
  [TestClass]
  public class OracleHelperTest
  {
    [TestMethod]
    public void TestGetUsers()
    {
      var results = OracleHelper.Get<User>("SELECT * FROM users");
      Assert.IsNotNull(results);
    }
  }
}
