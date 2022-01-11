using Microsoft.EntityFrameworkCore;
using NewsAppNet.Data;

namespace NewsAppNet.Services
{
    public class DatabaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                // Takes all of our migrations files and apply them against the database in case they are not implemented
                serviceScope.ServiceProvider.GetService<NewsAppDbContext>().Database.Migrate();
            }
        }
    }
}
