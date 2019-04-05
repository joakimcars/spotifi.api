using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{


    public class SpotifyExternalUrls
    {

        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
}