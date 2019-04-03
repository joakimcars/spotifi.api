using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchTrackResponse
    {
        [JsonProperty("tracks")]
        public SpotifySearchTrackCollection Tracks { get; set; }
    }
}
