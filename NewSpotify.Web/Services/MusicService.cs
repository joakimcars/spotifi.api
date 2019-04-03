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

        public async Task<List<SpotifyTrack>> GetRecommendationsAsync(List<string> tracks)
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
            var artistIds = new List<string>();
            foreach (var track in tracks)
            {
                var fullTrack = await GetTracksAsync(track);
                var artistId = fullTrack.Artists.FirstOrDefault()?.Id;
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
            var client = GetDefaultClient();
            string endpoint = $"v1/tracks/{trackId}";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);

            var trackResponse = JsonConvert.DeserializeObject<SpotifyTrack>(response);

            return trackResponse;
        }

        public async Task<List<SpotifyArtist>> GetRelatedArtistAsync(string artistId)
        {
            var client = GetDefaultClient();
            string endpoint = $"v1/artists/{artistId}/related-artists";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);

            var trackResponse = JsonConvert.DeserializeObject<SpotifyArtistResponse>(response);
            var topArtists = (from t in trackResponse.Artists
                orderby t.Popularity descending
                select t).Take(5);

            return topArtists.ToList();
        }

        public async Task<List<SpotifyTrack>> GetTopTracksForArtistAsync(string artistId)
        {
            var client = GetDefaultClient();
            string endpoint = $"v1/artists/{artistId}/top-tracks?country=ES";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);

            var topTrackResponse = JsonConvert.DeserializeObject<SpotifyTopTracksResponse>(response);
            var topTracks = (from t in topTrackResponse.Tracks
                orderby t.Popularity descending
                select t).Take(4);

            return topTracks.ToList();
        }

        public async Task<List<SpotifyTrack>> GetRecommendationByTrackFeatureAsync(List<string> tracks)
        {
            var trackFeatureList = new List<SpotifyTrackFeaturesResponse>();
            foreach (var track in tracks)
            {
                var temp = await GetTrackFeaturesAsync(track);
                trackFeatureList.Add(temp);
            }

            var avDanceAbility = trackFeatureList.Average(t => t.Danceability);
            var avAcousticness = trackFeatureList.Average(t => t.Acousticness);
            var avEnergy = trackFeatureList.Average(t => t.Energy);
            var avInstrumentalness = trackFeatureList.Average(t => t.Instrumentalness);

            var recommendations = await GetRecommendationsByFeaturesAsync(avDanceAbility, avAcousticness, avEnergy, avInstrumentalness, tracks);

            var topTracks = (from t in recommendations.Tracks
                orderby t.Popularity descending
                select t).Take(20);

            return topTracks.ToList();
        }

        public async Task<SpotifyTrackFeaturesResponse> GetTrackFeaturesAsync(string trackId)
        {
            var client = GetDefaultClient();
            string endpoint = $"v1/audio-features/{trackId}";
            var url = new Url(endpoint);
            var response = await client.GetStringAsync(url);
            var trackFeaturesResponse = JsonConvert.DeserializeObject<SpotifyTrackFeaturesResponse>(response);

            return trackFeaturesResponse;
        }

        public async Task<SpotifyRecomendationsresponse> GetRecommendationsByFeaturesAsync(double danceability, double acousticness, double energy, double instrumentalness, List<string> tracks )
        {
            var client = GetDefaultClient();
            var url = new Url("/v1/recommendations");

            url = url.SetQueryParam("target_danceability", danceability);
            url = url.SetQueryParam("target_acousticness", acousticness);
            url = url.SetQueryParam("target_energy", energy);
            url = url.SetQueryParam("target_instrumentalness", instrumentalness);
            url = url.SetQueryParam("min_popularity", 50);

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