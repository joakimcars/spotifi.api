using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyVideoThumbnail
    {

        [JsonProperty("url")]
        public object Url { get; set; }
    }
}