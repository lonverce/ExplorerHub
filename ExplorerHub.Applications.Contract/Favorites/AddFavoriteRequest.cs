namespace ExplorerHub.Applications.Favorites
{
    public class AddFavoriteRequest
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public byte[] Icon { get; set; }
    }
}