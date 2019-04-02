using Newtonsoft.Json;
using System.Collections.Generic;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifyArtist
    {

        [JsonProperty("external_urls")] public SpotifyExternalUrls ExternalUrls { get; set; }

        [JsonProperty("genres")] public IList<object> Genres { get; set; }

        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("images")] public IList<SpotifyImage> Images { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("popularity")] public int Popularity { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }
}