using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchTrackCollection
    {
        [JsonProperty("items")]
        public IList<SpotifyTrack> Items { get; set; }
    }
}
