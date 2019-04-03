using System.Collections.Generic;
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
        private readonly MusicService _service;
        private readonly ModelConverterService _converterService;

        public HomeController(MusicService service, ModelConverterService converterService)
        {
            _service = service;
            _converterService = converterService;
        }

        public async Task<IActionResult> Index(string trackId, string songName, string imageUrl, string bandName)
        {
            const string likeListSessionKey = "_likeList";

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
                return RedirectToAction("RecommendationsBySpotify", new { trackIds = likeListIds });
            }

            var categories = await _service.SearchCategoriesASync();

            if (categories == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            var indexVm = _converterService.ConvertToIndexVm(categories, likedSongList);
            return View(indexVm);
        }

        public async Task<IActionResult> RecommendationsBySpotify(List<string> trackIds)
        {

            var recommendations = await _service.GetRecommendationsAsync(trackIds);

            if (recommendations == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            var recommendationsVm = _converterService.ConvertToRecommendationsVm(recommendations);

            return View("Recommendations", recommendationsVm);
        }

        public async Task<IActionResult> RecommendationsByRelated(List<string> trackIds)
        {

            var recommendations = await _service.GetRecommendationByRelatedAsync(trackIds);

            if (recommendations == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }

            var recommendationsVm = _converterService.ConvertToRecommendationsVm(recommendations);

            return View("Recommendations", recommendationsVm);
        }

        public async Task<IActionResult> RecommendationsByTrackFeature(List<string> trackIds)
        {

            var recommendations = await _service.GetRecommendationByTrackFeatureAsync(trackIds);

            if (recommendations == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }

            var recommendationsVm = _converterService.ConvertToRecommendationsVm(recommendations);

            return View("Recommendations", recommendationsVm);
        }

        public IActionResult RemoveSong(string trackId)
        {
            const string likeListSessionKey = "_likeList";
            var likedSongList = GetSessionState();

            likedSongList.RemoveAll(t => t.TrackId == trackId);
            var json = JsonConvert.SerializeObject(likedSongList);
            HttpContext.Session.SetString(likeListSessionKey, json);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult QuickRecommendation(string target)
        {
            var likedSongList = GetSessionState();
            var likeListIds = new List<string>();
            HttpContext.Session.Clear();
            foreach (var track in likedSongList)
            {
                likeListIds.Add(track.TrackId);
            }

            return RedirectToAction(target, "Home", new { trackIds = likeListIds });
        }

        public IActionResult NewSearch()
        {
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