using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NewSpotify.Models.Models.Spotify;
using Newtonsoft.Json;
using NewSpotify.Models.Models.HelperModels;

namespace NewSpotify.Web.Services
{
    public class MusicService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _config;
        private const string CategoryCacheKey = "_categoryCache";

        protected const string BaseUrl = "https://api.spotify.com/";

        public MusicService(IMemoryCache memoryCache, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _config = config;
        }

        private HttpClient GetDefaultClient()
        {
            var authHandler = new SpotifyAuthClientCredentialsHttpMessageHandler(
                _config["ClientId"],
                _config["ClientSecret"],
                new HttpClientHandler(),
                _memoryCache
            );

            var client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            return client;
        }

        public async Task<T> GetResponseAsync<T>(Url url, string cacheKey = null) 
        {
            var client = GetDefaultClient();
            var response = await client.GetStringAsync(url);
            var tracksResponse = JsonConvert.DeserializeObject<T>(response);
            if (cacheKey != null)
            {
                _memoryCache.Set(cacheKey, tracksResponse, TimeSpan.FromHours(1));
            }
            return tracksResponse;
        }

        public async Task<SpotifySearchTrackResponse> SearchTracksAsync(string trackName)
        {
            if (_memoryCache.TryGetValue(trackName, out var cacheValue))
            {
                return cacheValue as SpotifySearchTrackResponse;
            }

            var url = new Url("/v1/search");
            url = url.SetQueryParam("q", trackName);
            url = url.SetQueryParam("type", "track");

            try
            {
                return await GetResponseAsync<SpotifySearchTrackResponse>(url, trackName);
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
            
            var url = new Url("/v1/browse/categories");
            try
            {
                return await GetResponseAsync<SpotifySearchCategoriesResponse>(url, CategoryCacheKey);
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
            
            var endpoint = $"/v1/browse/categories/{categoryId}/playlists";
            var url = new Url(endpoint);

            try
            {
                var playListResponse = await GetResponseAsync<SpotifySearchPlayListResponse>(url, categoryId);
                return playListResponse.Playlists.Items.Count != 0 ? playListResponse : null;
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
            
            var endpoint = $"v1/playlists/{playListId}/tracks";
            var url = new Url(endpoint);
            try
            {
                return await GetResponseAsync<SpotifyTrackresponse>(url, playListId);
            }
            catch (HttpRequestException)
            {
                return null;
            }
            
        }

        public async Task<List<SpotifyTrack>> GetRecommendationsAsync(List<string> tracks)
        {
            var url = new Url("/v1/recommendations");

            foreach (var track in tracks)
            {
                url = url.SetQueryParam("seed_tracks", track);
            }

            try
            {
                var recommendationsResponse = await GetResponseAsync<SpotifyRecomendationsresponse>(url);

                var topTracks = (from t in recommendationsResponse.Tracks
                                 orderby t.Popularity descending
                                 select t).Take(20);

                return topTracks.ToList();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<List<SpotifyTrack>> GetRecommendationByRelatedAsync(List<string> tracks)
        {
            //ändra till en concurrent bag
            //om det ens behöver sparas
            var artistIds = new List<string>();

            foreach (var track in tracks)
            {
                var fullTrack = await GetTracksAsync(track);
                var artistId = fullTrack.Artists.FirstOrDefault()?.Id;
                //anropa getrelatedartist
                artistIds.Add(artistId);
            }

            var relatedArtists = new List<SpotifyArtist>();
            foreach (var artist in artistIds)
            {
                var temp = await GetRelatedArtistAsync(artist);
                foreach (var relatedArtist in temp)
                {
                    relatedArtists.Add(relatedArtist);
                }
            }

            var topTracks = new List<SpotifyTrack>();
            foreach (var artist in relatedArtists)
            {
                var temp = await GetTopTracksForArtistAsync(artist.Id);
                foreach (var topTrack in temp)
                {
                    topTracks.Add(topTrack);
                }
            }

            var recommendations = (from t in topTracks
                orderby t.Popularity descending
                select t).Take(20);

            return recommendations.ToList();
        }

        public async Task<SpotifyTrack> GetTracksAsync(string trackId)
        {
            var endpoint = $"v1/tracks/{trackId}";
            var url = new Url(endpoint);
            
            return await GetResponseAsync<SpotifyTrack>(url);
        }

        public async Task<List<SpotifyArtist>> GetRelatedArtistAsync(string artistId)
        {
            var endpoint = $"v1/artists/{artistId}/related-artists";
            var url = new Url(endpoint);
            
            var trackResponse = await GetResponseAsync<SpotifyArtistResponse>(url);

            var topArtists = (from t in trackResponse.Artists
                orderby t.Popularity descending
                select t).Take(5);

            return topArtists.ToList();
        }

        public async Task<List<SpotifyTrack>> GetTopTracksForArtistAsync(string artistId)
        {
            var endpoint = $"v1/artists/{artistId}/top-tracks?country=ES";
            var url = new Url(endpoint);
           
            var topTrackResponse = await GetResponseAsync<SpotifyTopTracksResponse>(url);

            var topTracks = (from t in topTrackResponse.Tracks
                orderby t.Popularity descending
                select t).Take(4);

            return topTracks.ToList();
        }

        public async Task<List<SpotifyTrack>> GetRecommendationByTrackFeatureAsync(List<string> tracks)
        {
            try
            {
                var trackFeatureList = new List<SpotifyTrackFeaturesResponse>();
                foreach (var track in tracks)
                {
                    var temp = await GetTrackFeaturesAsync(track);
                    trackFeatureList.Add(temp);
                }

                var features = new TrackFeatures
                {
                    Danceability = trackFeatureList.Average(t => t.Danceability),
                    Acousticness = trackFeatureList.Average(t => t.Acousticness),
                    Energy = trackFeatureList.Average(t => t.Energy),
                    Instrumentalness = trackFeatureList.Average(t => t.Instrumentalness),
                    Tracks = tracks
                };

                var recommendations = await GetRecommendationsByFeaturesAsync(features);

                var topTracks = (from t in recommendations.Tracks
                    orderby t.Popularity descending
                    select t).Take(20);

                return topTracks.ToList();
            }

            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<SpotifyTrackFeaturesResponse> GetTrackFeaturesAsync(string trackId)
        {
            var endpoint = $"v1/audio-features/{trackId}";
            var url = new Url(endpoint);
            
            return await GetResponseAsync<SpotifyTrackFeaturesResponse>(url);
        }

        public async Task<SpotifyRecomendationsresponse> GetRecommendationsByFeaturesAsync(TrackFeatures features )
        {
            var url = new Url("/v1/recommendations");

            url = url.SetQueryParam("target_danceability", features.Danceability);
            url = url.SetQueryParam("target_acousticness", features.Acousticness);
            url = url.SetQueryParam("target_energy", features.Energy);
            url = url.SetQueryParam("target_instrumentalness", features.Instrumentalness);
            url = url.SetQueryParam("min_popularity", 50);

            foreach (var track in features.Tracks)
            {
                url = url.SetQueryParam("seed_tracks", track);
            }

            return await GetResponseAsync<SpotifyRecomendationsresponse>(url);
        }
    }
}