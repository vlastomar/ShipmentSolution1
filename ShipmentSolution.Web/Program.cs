using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Services.Core;
using ShipmentSolution.Services.Core.Interfaces;

namespace ShipmentSolution.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register services
            builder.Services.AddScoped<IShipmentService, ShipmentService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IDeliveryService, DeliveryService>();
            builder.Services.AddScoped<IMailCarrierService, MailCarrierService>();
            builder.Services.AddScoped<IRouteService, RouteService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // 🔐 Seed roles and assign admin
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                string[] roles = { "Administrator", "RegisteredUser" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var configuration = builder.Configuration;

                string? adminEmail = configuration["AdminUser:Email"];
                string? adminPassword = configuration["AdminUser:Password"];

                if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                {
                    throw new InvalidOperationException("Admin credentials are not properly configured in appsettings.");
                }

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Administrator"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }

            // Middleware Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error500");
                app.UseStatusCodePagesWithReExecute("/Home/Error{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
