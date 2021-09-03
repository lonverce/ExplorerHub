using System.Threading.Tasks;
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

        public async Task InitializeAppComponentsAsync()
        {
            var db = _dbContext.Database;
            await db.MigrateAsync();
        }

        public Task ReleaseAppComponentAsync()
        {
            return Task.CompletedTask;
        }
    }
}
