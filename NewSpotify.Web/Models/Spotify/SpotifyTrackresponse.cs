using System.Collections.Generic;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyTrackresponse
    {

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public IList<SpotifyItem> Items { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}