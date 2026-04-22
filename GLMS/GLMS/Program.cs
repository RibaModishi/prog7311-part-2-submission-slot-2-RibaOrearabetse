using GLMS.Data;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

namespace GLMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // hook up ef core + sql server - connection string is in appsettings.json under ConnectionStrings:DefaultConnection
            builder.Services.AddDbContext<GlmsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<Services.ServiceRequestWorkflowService>();
            builder.Services.AddScoped<Services.ContractQueryService>();

            // Section 3 (Frankfurter / HttpClient): assisted by DeepSeek AI — i reviewed. Typed client + BaseAddress + timeout.
            builder.Services.AddHttpClient<Services.UsdZarExchangeService>(client =>
            {
                client.BaseAddress = new Uri("https://api.frankfurter.app/");
                client.Timeout = TimeSpan.FromSeconds(20);
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

// References (accessed 18-20 Apr 2026):
// - DbContext configuration & DI: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
// - ASP.NET Core dependency injection: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
// - IHttpClientFactory in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
// - Authorization in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/introduction
