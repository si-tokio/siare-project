namespace siare.Server.Helpers
{
  public static class SessionHelper
  {
    private static Random random = new Random();

    public static string GenerateSessionId()
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      char[] sessionId = new char[10];

      lock (random)
      {
        for (int i = 0; i < 10; i++)
        {
          sessionId[i] = chars[random.Next(chars.Length)];
        }
      }

      return new string(sessionId);
    }

    public static DateTime GetExpirationDate(int days)
    {
      return DateTime.Now.AddDays(days);
    }
  }
}
