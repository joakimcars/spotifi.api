using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyRecomendationsresponse
    {
        [JsonProperty("tracks")]
        public IList<SpotifyTrack> Tracks { get; set; }

        [JsonProperty("seeds")]
        public IList<SpotifySeed> Seeds { get; set; }
    }
}
