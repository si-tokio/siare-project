using siare.Server.Helpers;
using siare.Shared.Entities;

namespace siare.Test.Server.Helpers
{
  [TestClass]
  public class OracleHelperTest
  {
    [TestMethod]
    public async void TestGetUsers()
    {
      var results = await OracleHelper.GetAsync<User>("SELECT * FROM users");
      Assert.IsNotNull(results);
    }
  }
}
