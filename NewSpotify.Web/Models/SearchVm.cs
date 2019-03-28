using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace NewSpotify.Web.Models
{
    public class SuggestionsByCategoryVm
    {
        public string CategoryName { get; set; }
        public IList<Icon> CategoryIcons { get; set; }
        public IList<Artist> Type { get; set; }
    }


    public class TopSuggestions
    {
        public string Typ { get; set; }
    }

    public class SearchArtistResponse
    {
        [JsonProperty("artists")]
        public SearchArtistCollection Artists { get; set; }
    }

    public class SearchCategoriesResponse
    {
        [JsonProperty("categories")]
        public SearchCategoriesCollection Categories { get; set; }
    }

    public class SearchPlayListResponse
    {
        [JsonProperty("playlists")]
        public SearchPlayListsCollection Playlists { get; set; }
    }

    public class SearchPlayListsCollection
    {
        [JsonProperty("items")]
        public IList<Playlists> Items { get; set; }
    }
    public class SearchCategoriesCollection
    {
        [JsonProperty("items")]
        public IList<Categories> Items { get; set; }
    }

    public class IndexVm
    {
        public IList<Category> Categories { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public IList<Icon> Icons { get; set; }
        public IList<Track> PlayList { get; set; }
    }

    public class Track
    {

    }

    public class Categories
    {
        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("icons")] public IList<Icon> Icons { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }

    public class SearchArtistCollection
    {
        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("items")] public IList<Artist> Items { get; set; }

        [JsonProperty("limit")] public int Limit { get; set; }

        [JsonProperty("next")] public object Next { get; set; }

        [JsonProperty("offset")] public int Offset { get; set; }

        [JsonProperty("previous")] public object Previous { get; set; }

        [JsonProperty("total")] public int Total { get; set; }
    }

    public class Artist
    {
        [JsonProperty("external_urls")] public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("genres")] public IList<object> Genres { get; set; }

        [JsonProperty("href")] public string Href { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("images")] public IList<Image> Images { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("popularity")] public int Popularity { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("uri")] public string Uri { get; set; }
    }

    public class ExternalUrls
    {
        [JsonProperty("spotify")] public string Spotify { get; set; }
    }

    public class Image
    {
       [JsonProperty("url")] public string Url { get; set; }
    }

    public class Icon
    {
        [JsonProperty("url")] public string Url { get; set; }
    }

    public class Owner
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class Tracks
    {

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class Item
    {

        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public IList<Image> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("primary_color")]
        public object PrimaryColor { get; set; }

        [JsonProperty("public")]
        public object Public { get; set; }

        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public class Playlists
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("images")]
        public IList<Image> Images { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("items")]
        public IList<Item> Items { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class Example
    {

        [JsonProperty("playlists")]
        public Playlists Playlists { get; set; }
    }



}