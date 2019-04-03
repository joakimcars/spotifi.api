using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyTopTracksResponse
    {
        [JsonProperty("tracks")]
        public List<SpotifyTrack> Tracks { get; set; }
    }
}
