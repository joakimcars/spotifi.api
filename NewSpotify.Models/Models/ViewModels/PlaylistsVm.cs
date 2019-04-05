using System.Collections.Generic;

namespace NewSpotify.Models.Models.ViewModels
{
    public class PlaylistsVm
    {
        public List<SelectedSongsVm> SelectedSongs { get; set; }
        public List<PlayListVm> PlayLists { get; set; }
    }
}
