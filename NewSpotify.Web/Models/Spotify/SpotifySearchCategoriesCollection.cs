using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchCategoriesCollection
    {
        [JsonProperty("items")]
        public IList<SpotifyCategories> Items { get; set; }
    }
}
