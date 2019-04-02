using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchArtistResponse
    {
        [JsonProperty("artists")]
        public SpotifySearchArtistCollection Artists { get; set; }
    }
}
