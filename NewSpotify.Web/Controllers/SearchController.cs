using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Web.Models;
using NewSpotify.Web.Services;
using Newtonsoft.Json;

namespace NewSpotify.Web.Controllers
{
    public class SearchController : Controller
    {
        readonly MusicService service;
        readonly ModelConverterService converterService;

        public SearchController(MusicService service, ModelConverterService converterService)
        {
            this.service = service;
            this.converterService = converterService;
        }
        
        public async Task<IActionResult> SearchResults(string searchString) 
        {
            var searchResults = await service.SearchArtistsAsync(searchString);
            return View(searchResults);
        }

        public async Task<IActionResult> PlayLists(string id)
        {
           
            var likeList = GetSessionState();
            var playLists = await service.GetPlayListsByCategoryAsync(id);
            playLists.SelectedSongs = likeList;
            var playListVm = converterService.ConvertToPlaylistVm(playLists, likeList);
            return View(playListVm);
        }

        public async Task<IActionResult> Tracks(string id)
        {
            var likeList = GetSessionState();
            var tracks = await service.GetTracksForPlaylistAsync(id);
            tracks.SelectedSongs = likeList;
            var tracksVm = converterService.ConvertToTracksVm(tracks, likeList);
            return View(tracksVm);
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