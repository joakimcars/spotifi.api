using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyVideoThumbnail
    {

        [JsonProperty("url")]
        public object Url { get; set; }
    }
}