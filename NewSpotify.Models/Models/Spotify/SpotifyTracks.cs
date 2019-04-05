using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyTracks
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
