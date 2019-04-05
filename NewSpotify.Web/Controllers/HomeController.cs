﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Models.Models.StateManagerModels;
using NewSpotify.Web.Services;
using Newtonsoft.Json;

namespace NewSpotify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicService _service;
        private readonly ModelConverterService _converterService;
        private readonly LikedSongsService _likedSongsService;

        public HomeController(MusicService service, ModelConverterService converterService, LikedSongsService likedSongsService)
        {
            _service = service;
            _converterService = converterService;
            _likedSongsService = likedSongsService;
        }

        public async Task<IActionResult> Index()
        {
            var likedSongList = GetSessionState();

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