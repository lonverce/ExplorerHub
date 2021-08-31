using System;
using System.Collections.Generic;
using System.Linq;
using ExplorerHub.Domain.Favorites;
using ExplorerHub.Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore.Favorites
{
    public class FavoriteDbContext : DbContext, IFavoriteRepository
    {
        private readonly string _dbFilePath;

        public DbSet<Favorite> Favorites { get; set; }

        [InjectProperty]
        public IEventBus EventBus { get; set; }

        public FavoriteDbContext(string dbFilePath)
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
            optionsBuilder.UseSqlite($"Data Source={_dbFilePath}");
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entry => entry.Entity.HasDomainEvents())
                .Select(entry => entry.Entity)
                .ToArray();

            var result = base.SaveChanges();

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

        public Favorite Add(Favorite favorite)
        {
            favorite = Favorites.Add(favorite).Entity;
            favorite.AddDomainEvent(new FavoriteAddedEventData(favorite.Id));
            return favorite;
        }

        public void Delete(Favorite favorite)
        {
            Favorites.Remove(favorite);
            favorite.AddDomainEvent(new FavoriteRemovedEventData(favorite.Id));
        }

        public Favorite FindById(Guid id)
        {
            return Favorites.AsTracking().FirstOrDefault(favorite => favorite.Id == id);
        }

        public List<Favorite> GetAll()
        {
            return Favorites.AsTracking().ToList();
        }

        public override void Dispose()
        {
            if (ChangeTracker.Entries().Any())
            {
                SaveChanges();
            }

            base.Dispose();
        }
    }
}
