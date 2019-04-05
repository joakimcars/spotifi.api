using System.Collections.Generic;

namespace NewSpotify.Models.Models.ViewModels
{
    public class TracksVm
    {
        public List<TrackVm> Tracks { get; set; }
        public List<SelectedSongsVm> SelectedSongs { get; set; }

        public string PlaylistId { get; set; }
    }
}
