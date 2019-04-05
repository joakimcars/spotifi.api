using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}