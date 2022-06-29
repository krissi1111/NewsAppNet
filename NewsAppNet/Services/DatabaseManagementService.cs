using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data;
using NewsAppNet.Models.DataModels;

namespace NewsAppNet.Services
{
    public class DatabaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app, bool docker)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                // Takes all of our migrations files and apply them against the database in case they are not implemented
                //serviceScope.ServiceProvider.GetService<NewsAppDbContext>().Database.EnsureCreated();
                DbContext dbContext;
                if (docker)
                {
                    dbContext = serviceScope.ServiceProvider.GetService<NewsAppDbContext>();
                    dbContext.Database.Migrate();
                }
                else
                {
                    dbContext = serviceScope.ServiceProvider.GetService<NewsAppDbContext>();
                    dbContext.Database.EnsureCreated();
                }
                UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole<int>> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();

                DbSeedService seedService = new(dbContext, userManager, roleManager);
                seedService.SeedDb();
            }
        }
    }
}
