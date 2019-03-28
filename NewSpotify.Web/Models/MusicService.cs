using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models
{
    public class MusicService
    {
        private readonly IMemoryCache _memoryCache;
        private const string clientId = "996d0037680544c987287a9b0470fdbb";
        private const string clientSecret = "5a3c92099a324b8f9e45d77e919fec13";

        protected const string BaseUrl = "https://api.spotify.com/";

        public MusicService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        
        private HttpClient GetDefaultClient()
        {
            var authHandler = new SpotifyAuthClientCredentialsHttpMessageHandler(
                clientId,
                clientSecret,
                new HttpClientHandler(),
                _memoryCache
                );

            var client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return client;
        }

        public async Task<SearchArtistResponse> SearchArtistsAsync(string artistName, int? limit = null, int? offset = null)
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/search");
            url = url.SetQueryParam("q", artistName);
            url = url.SetQueryParam("type", "artist");

            if (limit != null)
                url = url.SetQueryParam("limit", limit);

            if (offset != null)
                url = url.SetQueryParam("offset", offset);

            var response = await client.GetStringAsync(url);

            var artistResponse = JsonConvert.DeserializeObject<SearchArtistResponse>(response);
            return artistResponse;
        }

        public async Task<SearchCategoriesResponse> SearchCategoriesASync()
        {
            var client = GetDefaultClient();

            var url = new Url("/v1/browse/categories");
            var response = await client.GetStringAsync(url);
            var categoriesResponse = JsonConvert.DeserializeObject<SearchCategoriesResponse>(response);
           
            return categoriesResponse;
        }

        public async Task<SearchPlayListResponse> GetPlayListsByCategoryAsync(string categoryId)
        {
            var client = GetDefaultClient();
            string endpoint = $"/v1/browse/categories/{categoryId}/playlists";

            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);
            var playListResponse = JsonConvert.DeserializeObject<SearchPlayListResponse>(response);

            return playListResponse;
        }

        public async void GetTracksForPlaylistAsync(string playListId)
        {
            var client = GetDefaultClient();
            string endpoint = $"v1/playlists/{playListId}/tracks";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);

            GetTracksForPlaylistAsync("");
        }


    }
}
