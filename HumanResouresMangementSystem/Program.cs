using HumanResouresMangementSystem.Core.Authorization;
using HumanResouresMangementSystem.Core.Services;
using HumanResouresMangementSystem.DAL.Interfaces;
using HumanResouresMangementSystem.DAL.Services;
using HumanResouresMangementSystem.DAL.UnitOfWork.HumanResouresMangementSystem.DAL.Repository;
using HumanResouresMangementSystem.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HumanResouresMangementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection")));

            // Permission Service
            builder.Services.AddScoped<
                IPermissionService,
                PermissionService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Permission Policy Provider
            builder.Services.AddSingleton<
                IAuthorizationPolicyProvider,
                PermissionPolicyProvider>();

            // Permission Authorization Handler
            builder.Services.AddScoped<
                IAuthorizationHandler,
                PermissionAuthorizationHandler>();

            // Authentication
            builder.Services
                .AddAuthentication(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            // Authorization
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // =========================
            // Seed Data
            // =========================

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                await DbInitializer.SeedAsync(context);
            }

            // =========================
            // HTTP Request Pipeline
            // =========================

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // Authentication MUST come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}