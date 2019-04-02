using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyIcon
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
