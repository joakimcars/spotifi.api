using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewSpotify.Web.Models;

namespace NewSpotify.Web.Controllers
{
    public class HomeController : Controller
    {
        MusicService service;

        public HomeController(MusicService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await service.SearchCategoriesASync();
            return View(categories);
        }
    }
}