﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public List<string> GetLikedSongsIds()
        {
            var likedSongList = GetLikedSongs();
            var idList = likedSongList.Select(s => s.TrackId);
                
            return idList.ToList();
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

        public void RemoveSong(string trackId)
        {
            var likeList = GetLikedSongs();

            likeList.RemoveAll(t => t.TrackId == trackId);
            var json = JsonConvert.SerializeObject(likeList);
            _httpContextAccessor.HttpContext.Session.SetString(LikeListSessionKey, json);
        }

        public void Clear()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
