using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorerHub.EfCore
{
    public abstract class AbstractRepository : IDisposable, IAsyncDisposable
    {
        public ExplorerHubDbContext DbContext { get; }

        protected AbstractRepository(ExplorerHubDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            dbContext.Database.EnsureCreated();
            DbContext = dbContext;
        }

        public void Dispose()
        {
            if (DbContext.ChangeTracker.Entries().Any())
            {
                DbContext.SaveChanges();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (DbContext.ChangeTracker.Entries().Any())
            {
                await DbContext.SaveChangesAsync();
            }
        }
    }
}
