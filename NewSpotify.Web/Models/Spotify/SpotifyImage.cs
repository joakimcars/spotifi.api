using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyImage
    {

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}