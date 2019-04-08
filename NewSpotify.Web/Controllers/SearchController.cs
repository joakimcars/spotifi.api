using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Models.Models.StateManagerModels;
using NewSpotify.Web.Services;
using Newtonsoft.Json;

namespace NewSpotify.Web.Controllers
{
    public class SearchController : Controller
    {
        readonly MusicService _service;
        readonly ModelConverterService _converterService;
        private readonly LikedSongsService _likedSongsService;

        public SearchController(MusicService service, ModelConverterService converterService, LikedSongsService likedSongsService)
        {
            _service = service;
            _converterService = converterService;
            _likedSongsService = likedSongsService;
        }
        
        public async Task<IActionResult> SearchResults(string searchString) 
        {
            var searchResults = await _service.SearchTracksAsync(searchString);
            if (searchResults == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            var likeList = _likedSongsService.GetLikedSongs();

            var resultVm = _converterService.ConvertToTracksVm(searchResults, likeList);
            return View("Tracks", resultVm);

            
        }

        public async Task<IActionResult> PlayLists(string id)
        {

            var likeList = _likedSongsService.GetLikedSongs();
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
            var likeList = _likedSongsService.GetLikedSongs();
            var tracks = await _service.GetTracksForPlaylistAsync(id);
            if (tracks == null)
            {
                Response.StatusCode = 500;
                return View("Error");
            }
            
            var tracksVm = _converterService.ConvertToTracksVm(tracks, likeList, id);
            return View(tracksVm);
        }

        public IActionResult AddLikedSong(SelectedSongItem song)
        {
            _likedSongsService.SetLikedSongs(song.TrackId, song.SongName, song.ImageUrl, song.BandName);

            return song.PlaylistId == null ? RedirectToAction("Index", "Home") : RedirectToAction("Tracks", "Search", new { id = song.PlaylistId });
        }
    }
}