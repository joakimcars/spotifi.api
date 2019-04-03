using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchTrackCollection
    {
        [JsonProperty("items")]
        public IList<SpotifyTrack> Items { get; set; }
    }
}
