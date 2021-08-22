using System;

namespace ExplorerHub.Models.Favorites
{
    public class FavoritePath
    {
        public FavoritePath(Guid id, string icon, string displayName, string parsingName)
        {
            Id = id;
            Icon = icon;
            DisplayName = displayName;
            ParsingName = parsingName;
        }

        public Guid Id { get; }

        public string Icon { get; }

        public string DisplayName { get; set; }

        public string ParsingName { get; }
    }
}
