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
        MusicService service;
        private List<string> likeList;
        public HomeController(MusicService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index(string trackId)
        {
            var likeListSessionKey = "_likeList";
            likeList = new List<string>();

            //försöker hämta det som finns i seessionen
            var likeListStringJson = HttpContext.Session.GetString(likeListSessionKey);

            //om det finns något i sessionen så sätter vi likeList till vad som finns där
            if (likeListStringJson != null)
            {
                likeList = JsonConvert.DeserializeObject<List<string>>(likeListStringJson);
            }
            
            //om likeList är mindre än 5 element så lägger vi till trackId i likeList
            if (likeList.Count < 5)
            {
                if (trackId != null)
                {
                    likeList.Add(trackId);

                    HttpContext.Session.SetString(likeListSessionKey, likeList.ToString());
                }
            }

            //if session likes.count > 4 
            //redirect to recommendations
            if (likeList.Count == 5)
            {
                return RedirectToAction("Recommendations", likeList);
            }


            var categories = await service.SearchCategoriesASync();
            return View(categories);
        }

        public async Task<IActionResult> Recommendations(List<string> trackIds)
        {
            var recommendations = await service.GetRecommendationsAsync(trackIds);
            return View(recommendations);
        }
    }
}