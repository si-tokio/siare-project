using Microsoft.AspNetCore.ResponseCompression;
using siare.Server.Repositories.Oracle.Users;

namespace siare
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddScoped<IUserRepository, UserRepository>();

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


      app.MapRazorPages();
      app.MapControllers();
      app.MapFallbackToFile("index.html");

      app.Run();
    }
  }
}