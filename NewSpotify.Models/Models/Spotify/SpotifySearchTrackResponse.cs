using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchTrackResponse
    {
        [JsonProperty("tracks")]
        public SpotifySearchTrackCollection Tracks { get; set; }
    }
}
