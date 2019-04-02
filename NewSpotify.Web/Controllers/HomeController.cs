using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NewSpotify.Web.Models;
using NewSpotify.Web.Services;
using Newtonsoft.Json;

namespace NewSpotify.Web.Controllers
{
    public class HomeController : Controller
    {
        readonly MusicService service;
        private readonly ModelConverterService converterService;
        private readonly MemoryCache cache;

        public HomeController(MusicService service, ModelConverterService converterService, MemoryCache cache)
        {
            this.service = service;
            this.converterService = converterService;
            this.cache = cache;
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
            if (categories == null)
            {
                //redirect to errorpage
            }
            var indexVM = converterService.ConvertToIndexVm(categories, likedSongList);
            return View(indexVM);
        }

        public async Task<IActionResult> Recommendations(List<string> trackIds)
        {
            var recommendations = await service.GetRecommendationsAsync(trackIds);
            var recommendationsVm = converterService.ConvertToRecommendationVm(recommendations);
            return View(recommendationsVm);
        }

        public IActionResult RemoveSong(string trackId)
        {
            var likeListSessionKey = "_likeList";
            var likedSongList = GetSessionState();

            likedSongList.RemoveAll(t => t.TrackId == trackId);
            var json = JsonConvert.SerializeObject(likedSongList);
            HttpContext.Session.SetString(likeListSessionKey, json);
            return RedirectToAction("Index", "Home");
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