using System.Collections.Generic;
using NewSpotify.Models.Models.StateManagerModels;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchCategoriesResponse
    {
        public List<SelectedSongItem> SelectedSongs { get; set; }

        [JsonProperty("categories")]
        public SpotifySearchCategoriesCollection Categories { get; set; }
    }
}