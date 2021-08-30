using System;
using ExplorerHub.Framework.Domain;

namespace ExplorerHub.Domain.Favorites
{
    public class Favorite : AggregateRoot
    {
        public Favorite(Guid id, string name, string location, byte[] icon)
        {
            Id = id;
            Name = name;
            Location = location;
            Icon = icon;
        }
        
        public string Name { get; }

        public string Location { get; }

        public byte[] Icon { get; }
    }
}
