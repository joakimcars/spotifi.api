using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchPlayListsCollection
    {
        [JsonProperty("items")]
        public IList<SpotifyPlaylists> Items { get; set; }
    }
}
