using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.ViewModels
{
    public class TrackVm
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public int Popularity { get; set; }
        public string Id { get; set; }
    }
}
