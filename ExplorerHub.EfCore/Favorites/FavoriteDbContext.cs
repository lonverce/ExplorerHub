using ExplorerHub.Domain.Favorites;
using Microsoft.EntityFrameworkCore;

namespace ExplorerHub.EfCore.Favorites
{
    public class FavoriteDbContext : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }

        public FavoriteDbContext(DbContextOptions<FavoriteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>(builder =>
            {
                builder.HasKey(favorite => favorite.Id);
                builder.Property(favorite => favorite.Id)
                    .ValueGeneratedOnAdd();

                builder.Property(favorite => favorite.Name)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.Property(favorite => favorite.Location)
                    .HasMaxLength(256)
                    .IsRequired();

                builder.HasIndex(favorite => favorite.Name)
                    .IsUnique();

                builder.Property(favorite => favorite.Icon)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
