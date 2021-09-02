using ExplorerHub.Framework;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore
{
    public class DbContextInitialization : IAppInitialization
    {
        private readonly ExplorerHubDbContext _dbContext;

        public DbContextInitialization(ExplorerHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeAppComponents()
        {
            var db = _dbContext.Database;
            db.Migrate();
        }

        public void ReleaseAppComponent()
        {
            
        }
    }
}
