using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using siare.Server.Repositories.Oracle.Sessions;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace siare.Server.Services.Auth
{
  // カスタム認証ハンドラー
  public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
  {
    private readonly ISessionRepository _sessionRepository;

    public CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ISessionRepository sessionRepository)
        : base(options, logger, encoder, clock)
    {
      _sessionRepository = sessionRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      // Cookieからセッション識別子を取得
      if (Request.Cookies.TryGetValue("SessionId", out string? sessionId))
      {
        // セッション識別子の検証
        var db_session = await _sessionRepository.GetSessionAsync(sessionId);
        if (!string.IsNullOrEmpty(db_session.SessionId))
        {
          var claims = new[] { new Claim(ClaimTypes.NameIdentifier, sessionId) };
          var identity = new ClaimsIdentity(claims, Scheme.Name);
          var principal = new ClaimsPrincipal(identity);
          var ticket = new AuthenticationTicket(principal, Scheme.Name);

          return AuthenticateResult.Success(ticket);
        }
      }

      return AuthenticateResult.Fail("Invalid session identifier");
    }
  }
}
