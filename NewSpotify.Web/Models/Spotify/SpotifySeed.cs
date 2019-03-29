using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySeed
    {

        [JsonProperty("initialPoolSize")]
        public int InitialPoolSize { get; set; }

        [JsonProperty("afterFilteringSize")]
        public int AfterFilteringSize { get; set; }

        [JsonProperty("afterRelinkingSize")]
        public int AfterRelinkingSize { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}
