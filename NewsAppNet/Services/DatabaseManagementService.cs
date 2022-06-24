using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data;

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
                }
                else
                {
                    dbContext = serviceScope.ServiceProvider.GetService<SqliteDbContext>();
                    dbContext.Database.EnsureCreated();
                }
                dbContext.Database.Migrate();
                DbSeedService seedService = new(dbContext);
                seedService.SeedDb();
            }
        }
    }
}
