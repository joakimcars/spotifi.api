using System.Collections.Generic;
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
        readonly MusicService _service;
        readonly ModelConverterService _converterService;

        public SearchController(MusicService service, ModelConverterService converterService)
        {
            _service = service;
            _converterService = converterService;
        }
        
        public async Task<IActionResult> SearchResults(string searchString) 
        {
            var searchResults = await _service.SearchTracksAsync(searchString);
            if (searchResults == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            var likeList = GetSessionState();

            var resultVm = _converterService.ConvertToTracksVm(searchResults, likeList);
            return View("Tracks", resultVm);

            
        }

        public async Task<IActionResult> PlayLists(string id)
        {
           
            var likeList = GetSessionState();
            var playLists = await _service.GetPlayListsByCategoryAsync(id);
            if (playLists == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }

            playLists.SelectedSongs = likeList;
            var playListVm = _converterService.ConvertToPlaylistVm(playLists, likeList);
            return View(playListVm);
        }

        public async Task<IActionResult> Tracks(string id)
        {
            if (id == "")
            {
                return RedirectToAction("Index", "Home");
            }

            var likeList = GetSessionState();
            var tracks = await _service.GetTracksForPlaylistAsync(id);
            if (tracks == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            tracks.SelectedSongs = likeList;
            var tracksVm = _converterService.ConvertToTracksVm(tracks, likeList, id);
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

        public IActionResult SetSessionState(string trackId, string songName, string imageUrl, string bandName, string playlistId)
        {
            const string likeListSessionKey = "_likeList";
            var likedSongList = GetSessionState();

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

            return playlistId == null ? RedirectToAction("Index", "Home") : RedirectToAction("Tracks", "Search", new { id = playlistId });
        }
    }
}