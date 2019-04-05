using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchCategoriesCollection
    {
        [JsonProperty("items")]
        public IList<SpotifyCategories> Items { get; set; }
    }
}
