using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Web.Models;
using NewSpotify.Web.Services;

namespace NewSpotify.Web.Controllers
{
    public class SearchController : Controller
    {
        MusicService service;

        public SearchController(MusicService service)
        {
            this.service = service;
        }
        
        public async Task<IActionResult> Result()
        {
            var recommendations = await service.SearchArtistsAsync("beatles");
            return View(recommendations);
        }

        public async Task<IActionResult> PlayLists(string id)
        {
            var playLists = await service.GetPlayListsByCategoryAsync(id);
            return View(playLists);
        }

        public async Task<IActionResult> Tracks(string id)
        {
            var tracks = await service.GetTracksForPlaylistAsync(id);
            return View(tracks);
        }
    }
}