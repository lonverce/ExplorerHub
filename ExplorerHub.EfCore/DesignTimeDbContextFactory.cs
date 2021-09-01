using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;

namespace ExplorerHub.EfCore
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ExplorerHubDbContext>
    {
        public ExplorerHubDbContext CreateDbContext(string[] args)
        {
            return new ExplorerHubDbContext(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "explorer-hub.design.db"));
        }
    }
}