using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NewSpotify.Contracts;
using NewSpotify.Models.Models.StateManagerModels;
using Newtonsoft.Json;

namespace NewSpotify.Web.Services
{
    public class LikedSongsService : ISessionStateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string LikeListSessionKey = "_likeList";
        public LikedSongsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<SelectedSongItem> GetLikedSongs()
        {
            var likeList = new List<SelectedSongItem>();

            var likeListStringJson = _httpContextAccessor.HttpContext.Session.GetString(LikeListSessionKey);

            if (likeListStringJson != null)
            {
                likeList = JsonConvert.DeserializeObject<List<SelectedSongItem>>(likeListStringJson);
            }

            return likeList;
        }

        public void SetLikedSongs(string trackId, string songName, string imageUrl, string bandName)
        {
            var likedSongList = GetLikedSongs();

            var selectedSong = new SelectedSongItem()
            {
                TrackId = trackId,
                SongName = songName,
                ImageUrl = imageUrl,
                BandName = bandName
            };

            likedSongList.Add(selectedSong);
            var json = JsonConvert.SerializeObject(likedSongList);
            _httpContextAccessor.HttpContext.Session.SetString(LikeListSessionKey, json);
        }
    }
}
