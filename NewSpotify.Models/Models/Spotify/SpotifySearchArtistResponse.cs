using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchArtistResponse
    {
        [JsonProperty("artists")]
        public SpotifySearchArtistCollection Artists { get; set; }
    }
}
