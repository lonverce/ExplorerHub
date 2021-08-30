using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;

namespace ExplorerHub.EfCore.Favorites
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FavoriteDbContext>
    {
        public FavoriteDbContext CreateDbContext(string[] args)
        {
            return new FavoriteDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "explorer-hub.design.db"));
        }
    }
}