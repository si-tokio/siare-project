using siare.Shared.Entities;

namespace siare.Server.Services.Auth
{
  public interface IAuthService
  {
    string GenerateJwtToken(string userId, string sessionId);
    (string userId, string sessionId) DecodeJwtToken(string jwtTokenCookie);
  }
}
