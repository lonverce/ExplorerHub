using ExplorerHub.Framework.Domain;

namespace ExplorerHub.Domain.Favorites
{
    public class Favorite : AggregateRoot
    {
        public Favorite(string name, string location, byte[] icon)
        {
            Name = name;
            Location = location;
            Icon = icon;
        }
        
        private Favorite() { }

        public string Name { get; }

        public string Location { get; }

        public byte[] Icon { get; }
    }
}
