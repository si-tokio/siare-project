using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace siare.Server.Services.Auth
{
  public sealed class AuthService : IAuthService
  {
    public string GenerateJwtToken(string userId, string sessionId)
    {
      var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim("sessionId", sessionId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_Here_It_Should_Be_Longer_Than_32_Characters")); // この文字列は32文字以上なければなりません。
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expires = DateTime.Now.AddDays(7);

      var token = new JwtSecurityToken(
          issuer: "your_domain",
          audience: "your_domain",
          claims: claims,
          expires: expires,
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string userId, string sessionId) DecodeJwtToken(string jwtTokenCookie)
    {
      var handler = new JwtSecurityTokenHandler();
      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "your_domain",
        ValidAudience = "your_domain",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key_Here_It_Should_Be_Longer_Than_32_Characters"))
      };

      SecurityToken validatedToken;
      try
      {
        handler.ValidateToken(jwtTokenCookie, validationParameters, out validatedToken);
      }
      catch (Exception)
      {
        throw new ArgumentException("Invalid JWT Token");
      }

      var jwtToken = validatedToken as JwtSecurityToken;
      var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
      var sessionIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sessionId");

      if (userIdClaim == null || sessionIdClaim == null)
      {
        throw new ArgumentException("Invalid JWT Token");
      }

      return (userIdClaim.Value, sessionIdClaim.Value);
    }
  }
}
