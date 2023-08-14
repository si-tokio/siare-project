using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using siare.Server.Repositories.Oracle.Sessions;
using siare.Server.Repositories.Oracle.Users;
using siare.Server.Services.Auth;

namespace siare
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddScoped<IAuthService, AuthService>();
      builder.Services.AddScoped<ISessionRepository, SessionRepository>();
      builder.Services.AddScoped<IUserRepository, UserRepository>();
      builder.Services.AddAuthentication("CustomAuth").AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomAuth", null);
      builder.Services.AddAuthorization(options =>
      {
        options.AddPolicy("CustomPolicy", policy =>
        {
          policy.RequireAuthenticatedUser();
          // 他のカスタム要件を追加することができます
        });
      });

      builder.Services.AddControllersWithViews();
      builder.Services.AddRazorPages();

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseWebAssemblyDebugging();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseBlazorFrameworkFiles();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapRazorPages();
      app.MapControllers();
      app.MapFallbackToFile("index.html");

      app.Run();
    }
  }
}