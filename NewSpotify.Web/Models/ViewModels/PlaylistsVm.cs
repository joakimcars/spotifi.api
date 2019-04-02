using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewSpotify.Web.Models.ViewModels
{
    public class PlaylistsVm
    {
        public List<SelectedSongsVm> SelectedSongs { get; set; }
        public List<PlayListVm> PlayLists { get; set; }
    }
}
