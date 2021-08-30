using System;

namespace ExplorerHub.Applications.Favorites
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public byte[] Icon { get; set; }
    }
}