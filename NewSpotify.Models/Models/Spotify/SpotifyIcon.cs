using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyIcon
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
