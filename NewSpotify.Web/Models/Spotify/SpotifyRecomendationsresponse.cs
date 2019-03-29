using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyRecomendationsresponse
    {
        [JsonProperty("tracks")]
        public IList<Track> Tracks { get; set; }

        [JsonProperty("seeds")]
        public IList<SpotifySeed> Seeds { get; set; }
    }
}
