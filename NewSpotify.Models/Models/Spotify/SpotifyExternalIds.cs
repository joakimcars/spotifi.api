using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyExternalIds
    {

        [JsonProperty("isrc")]
        public string Isrc { get; set; }
    }
}