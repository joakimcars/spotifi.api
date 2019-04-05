using System.Collections.Generic;

namespace NewSpotify.Models.Models.ViewModels
{
    public class IndexVm
    {
        public List<SelectedSongsVm> SelectedSongs { get; set; }
        public IList<CategoryVm> Categories { get; set; }
    }
}
