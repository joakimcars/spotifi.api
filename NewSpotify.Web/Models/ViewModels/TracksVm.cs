using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.ViewModels
{
    public class TracksVm
    {
        public List<TrackVm> Tracks { get; set; }
        public List<SelectedSongsVm> SelectedSongs { get; set; }
    }
}
