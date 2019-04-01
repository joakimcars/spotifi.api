using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Web.Models;
using NewSpotify.Web.Services;
using Newtonsoft.Json;

namespace NewSpotify.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly MusicService service;


        public HomeController(MusicService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index(string trackId, string songName, string imageUrl, string bandName)
        {
            var likeListSessionKey = "_likeList";

            var likedSongList = GetSessionState();

            if (likedSongList.Count < 5 && trackId != null)
            {
                var selectedSong = new SelectedSongItem()
                {
                    TrackId = trackId,
                    SongName = songName,
                    ImageUrl = imageUrl,
                    BandName = bandName
                };
                likedSongList.Add(selectedSong);
                var json = JsonConvert.SerializeObject(likedSongList);
                HttpContext.Session.SetString(likeListSessionKey, json);
            }

            if (likedSongList.Count == 5)
            {
                var likeListIds = new List<string>();
                foreach (var song in likedSongList)
                {
                    likeListIds.Add(song.TrackId);
                }
                return RedirectToAction("Recommendations", new { trackIds = likeListIds });
            }

            var categories = await service.SearchCategoriesASync();
            categories.SelectedSongs = likedSongList;
            return View(categories);
        }

        public async Task<IActionResult> Recommendations(List<string> trackIds)
        {
            var recommendations = await service.GetRecommendationsAsync(trackIds);
            return View(recommendations);
        }

        public IActionResult RemoveSong(string trackId)
        {
            //to be implemented
            return View();
        }

        public IActionResult QuickRecommendations()
        {
            var likedSongList = GetSessionState();
            var likeListIds = new List<string>();

            foreach (var track in likedSongList)
            {
                likeListIds.Add(track.TrackId);
            }

            return RedirectToAction("Recommendations", "Home", new { trackIds = likeListIds });
        }

        public IActionResult NewSearch()
        {
            //empty session state
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public List<SelectedSongItem> GetSessionState()
        {
            const string likeListSessionKey = "_likeList";
            var likeList = new List<SelectedSongItem>();

            var likeListStringJson = HttpContext.Session.GetString(likeListSessionKey);

            if (likeListStringJson != null)
            {
                likeList = JsonConvert.DeserializeObject<List<SelectedSongItem>>(likeListStringJson);
            }

            return likeList;
        }
    }
}