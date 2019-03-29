using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyExternalIds
    {

        [JsonProperty("isrc")]
        public string Isrc { get; set; }
    }
}