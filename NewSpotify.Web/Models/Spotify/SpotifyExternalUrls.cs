using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{


    public class SpotifyExternalUrls
    {

        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
}