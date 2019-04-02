using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}