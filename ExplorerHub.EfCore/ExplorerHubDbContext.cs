using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.Framework;
using ExplorerHub.Framework.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore
{
    public class ExplorerHubDbContext : DbContext
    {
        private readonly string _dbFilePath;

        public DbSet<Favorite> Favorites { get; set; }

        [InjectProperty]
        public IEventBus EventBus { get; set; }

        public ExplorerHubDbContext(string dbFilePath)
        {
            _dbFilePath = dbFilePath;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>(builder =>
            {
                builder.HasKey(favorite => favorite.Id);
                builder.Property(favorite => favorite.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(favorite => favorite.Name)
                    .HasMaxLength(FavoriteDomainConstants.MaxNameLength)
                    .IsRequired();

                builder.Property(favorite => favorite.Location)
                    .HasMaxLength(FavoriteDomainConstants.MaxUrlLength)
                    .IsRequired();

                builder.HasIndex(favorite => favorite.Location)
                    .IsUnique();

                builder.Property(favorite => favorite.Icon)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sb = new SqliteConnectionStringBuilder
            {
                DataSource = _dbFilePath,
            };

            optionsBuilder.UseSqlite(sb.ToString(), builder =>
            {
            });
        }

        public override int SaveChanges(bool accept)
        {
            var changedEntities = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entry => entry.Entity.HasDomainEvents())
                .Select(entry => entry.Entity)
                .ToArray();

            var result = base.SaveChanges(accept);

            foreach (var entity in changedEntities)
            {
                var events = entity.GetDomainEvents();

                foreach (var eventData in events)
                {
                    EventBus.PublishEvent(eventData);
                }

                entity.ClearDomainEvents();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changedEntities = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entry => entry.Entity.HasDomainEvents())
                .Select(entry => entry.Entity)
                .ToArray();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            foreach (var entity in changedEntities)
            {
                var events = entity.GetDomainEvents();

                foreach (var eventData in events)
                {
                    EventBus.PublishEvent(eventData);
                }

                entity.ClearDomainEvents();
            }
            return result;
        }
    }
}
