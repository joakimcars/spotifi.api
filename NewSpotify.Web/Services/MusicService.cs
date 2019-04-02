using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Caching.Memory;
using NewSpotify.Web.Models;
using NewSpotify.Web.Models.Spotify;
using Newtonsoft.Json;

namespace NewSpotify.Web.Services
{
    public class MusicService
    {
        private readonly IMemoryCache _memoryCache;
        private const string clientId = "996d0037680544c987287a9b0470fdbb";
        private const string clientSecret = "5a3c92099a324b8f9e45d77e919fec13";
        private const string CategoryCacheKey = "_categoryCache";
        private const string ArtistCacheKey = "_artistCache";
        private const string PlaylistCacheKey = "_playlistCache";

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
            
            if (_memoryCache.TryGetValue(CategoryCacheKey, out var cacheValue))
            {
                return cacheValue as SearchCategoriesResponse;
            }

            
            var client = GetDefaultClient();

            var url = new Url("/v1/browse/categories");
            try
            {
                var response = await client.GetStringAsync(url);
                var categoriesResponse = JsonConvert.DeserializeObject<SearchCategoriesResponse>(response);
                _memoryCache.Set(CategoryCacheKey, categoriesResponse, TimeSpan.FromHours(1));

                return categoriesResponse;
            }
            catch (HttpRequestException ex)
            {
                
            }

            return null;

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

        public async Task<SpotifyTrackresponse> GetTracksForPlaylistAsync(string playListId)
        {
            var client = GetDefaultClient();
            string endpoint = $"v1/playlists/{playListId}/tracks";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);

            var tracksResponse = JsonConvert.DeserializeObject<SpotifyTrackresponse>(response);


            return tracksResponse;
        }

        public async Task<SpotifyRecomendationsresponse> GetRecommendationsAsync(List<string> tracks)
        {
            var client = GetDefaultClient();
            var url = new Url("/v1/recommendations");

            foreach (var track in tracks)
            {
                url = url.SetQueryParam("seed_tracks", track);
            }

            var response = await client.GetStringAsync(url);
            var recommendationsResponse = JsonConvert.DeserializeObject<SpotifyRecomendationsresponse>(response);

            return recommendationsResponse;

        }

    }
}
