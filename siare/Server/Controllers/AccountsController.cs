using Microsoft.AspNetCore.Mvc;
using siare.Server.Helpers;
using siare.Server.Repositories.Oracle.Sessions;
using siare.Server.Repositories.Oracle.Users;
using siare.Server.Services.Auth;
using siare.Shared.Entities;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace siare.Server.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly ILogger<AccountsController> _logger;
    private readonly IAuthService _authService;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;

    public AccountsController(ILogger<AccountsController> logger,
                              IAuthService authService,
                              ISessionRepository sessionRepository,
                              IUserRepository userRepository)
    {
      _logger = logger;
      _authService = authService;
      _sessionRepository = sessionRepository;
      _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginViewModel model)
    {
      try
      {
        // 1. ユーザー認証: ここではmodelにはユーザーIDとパスワードが含まれると仮定します
        var user = await AuthenticateUser(model);
        if (user == null)
        {
          return Unauthorized();
        }

        // 2. セッションの作成と保存: ここでは新しいセッションIDを作成し、それをデータベースに保存します
        var sessionId = SessionHelper.GenerateSessionId();
        var expiresAt = SessionHelper.GetExpirationDate(7);
        await _sessionRepository.CreateSessionAsync(sessionId, user.UserId, expiresAt);

        // 3. JWTトークンの生成
        var token = _authService.GenerateJwtToken(user.UserId.ToString(), sessionId);

        // 4. 生成したトークンをクライアントに返す
        //return Ok(new { Token = token });
        // Set the JWT token in an HttpOnly cookie
        var cookieOptions = new CookieOptions
        {
          HttpOnly = true,
          Expires = expiresAt,
        };
        Response.Cookies.Append("jwtToken", token, cookieOptions);

        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

    private async Task<User?> AuthenticateUser(LoginViewModel model)
    {
      var user = await _userRepository.GetUserByIdAsync(model.UserId);
      if (user == null)
      {
        return null;
      }
      var passwordHash = HashHelper.HashPassword(model.Password!, user.Salt!);
      user = await _userRepository.GetUserByIdAndPasswordAsync(user.UserId, passwordHash);
      if (user == null)
      {
        return null;
      }
      return user;
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
      try
      {
        // 1. "jwtToken"というクッキーの内容を取得
        var jwtTokenCookie = Request.Cookies["jwtToken"];
        if (string.IsNullOrEmpty(jwtTokenCookie))
        {
          // Cookieが存在しない場合はUnauthorizedを返す
          return Unauthorized();
        }

        // 2. トークンからuserIdとsessionIdを取り出す
        var (userId, sessionId) = _authService.DecodeJwtToken(jwtTokenCookie);
        if (userId == null || sessionId == null)
        {
          // トークンが正しくデコードできない場合はUnauthorizedを返す
          return Unauthorized();
        }

        // 3. userIdとsessionIdをもとにsessionsテーブルから該当するsession情報を削除する
        await _sessionRepository.DeleteSessionAsync(sessionId, userId);

        // 4. クッキーを削除する
        Response.Cookies.Delete("jwtToken");

        // 5. OKを返す
        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex.Message);
        return StatusCode(500);
      }
    }

  }

  public class LoginViewModel
  {
    [Required, Range(1, 99999)]
    public int UserId { get; set; }

    [Required]
    public string? Password { get; set; }
  }
}
