using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyTopTracksResponse
    {
        [JsonProperty("tracks")]
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
