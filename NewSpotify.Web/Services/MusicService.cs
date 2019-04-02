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

        public async Task<SpotifySearchArtistResponse> SearchArtistsAsync(string artistName)
        {
            if (_memoryCache.TryGetValue(artistName, out var cacheValue))
            {
                return cacheValue as SpotifySearchArtistResponse;
            }

            var client = GetDefaultClient();

            var url = new Url("/v1/search");
            url = url.SetQueryParam("q", artistName);
            url = url.SetQueryParam("type", "artist");

            try
            {
                var response = await client.GetStringAsync(url);
                var artistResponse = JsonConvert.DeserializeObject<SpotifySearchArtistResponse>(response);
                _memoryCache.Set(artistName, artistResponse, TimeSpan.FromHours(1));
                return artistResponse;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<SpotifySearchCategoriesResponse> SearchCategoriesASync()
        {
            if (_memoryCache.TryGetValue(CategoryCacheKey, out var cacheValue))
            {
                return cacheValue as SpotifySearchCategoriesResponse;
            }

            var client = GetDefaultClient();
            var url = new Url("/v1/browse/categories");
            try
            {
                var response = await client.GetStringAsync(url);
                var categoriesResponse = JsonConvert.DeserializeObject<SpotifySearchCategoriesResponse>(response);
                _memoryCache.Set(CategoryCacheKey, categoriesResponse, TimeSpan.FromHours(1));

                return categoriesResponse;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<SpotifySearchPlayListResponse> GetPlayListsByCategoryAsync(string categoryId)
        {
            if (_memoryCache.TryGetValue(categoryId, out SpotifySearchPlayListResponse cacheValue))
            {
                return cacheValue;
            }

            var client = GetDefaultClient();
            string endpoint = $"/v1/browse/categories/{categoryId}/playlists";

            var url = new Url(endpoint);
            try
            {
                var response = await client.GetStringAsync(url);
                var playListResponse = JsonConvert.DeserializeObject<SpotifySearchPlayListResponse>(response);
                _memoryCache.Set(categoryId, playListResponse, TimeSpan.FromHours(1));
                return playListResponse;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<SpotifyTrackresponse> GetTracksForPlaylistAsync(string playListId)
        {
            if (_memoryCache.TryGetValue(playListId, out SpotifyTrackresponse cacheValue))
            {
                return cacheValue;
            }

            var client = GetDefaultClient();
            string endpoint = $"v1/playlists/{playListId}/tracks";
            var url = new Url(endpoint);
            try
            {
                var response = await client.GetStringAsync(url);
                var tracksResponse = JsonConvert.DeserializeObject<SpotifyTrackresponse>(response);
                _memoryCache.Set(playListId, tracksResponse, TimeSpan.FromHours(1));

                return tracksResponse;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<SpotifyRecomendationsresponse> GetRecommendationsAsync(List<string> tracks)
        {
            var client = GetDefaultClient();
            var url = new Url("/v1/recommendations");

            foreach (var track in tracks)
            {
                url = url.SetQueryParam("seed_tracks", track);
            }

            try
            {
                var response = await client.GetStringAsync(url);
                var recommendationsResponse = JsonConvert.DeserializeObject<SpotifyRecomendationsresponse>(response);

                return recommendationsResponse;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}