using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchCategoriesResponse
    {
        public List<SelectedSongItem> SelectedSongs { get; set; }

        [JsonProperty("categories")]
        public SpotifySearchCategoriesCollection Categories { get; set; }
    }
}