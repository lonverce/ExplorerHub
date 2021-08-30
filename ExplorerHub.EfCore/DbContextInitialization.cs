using ExplorerHub.EfCore.Favorites;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore
{
    public class DbContextInitialization : IAppInitialization
    {
        private readonly FavoriteDbContext _dbContext;

        public DbContextInitialization(FavoriteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeAppComponents()
        {
            _dbContext.Database.Migrate();
        }
    }
}
