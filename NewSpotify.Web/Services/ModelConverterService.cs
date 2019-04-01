using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewSpotify.Web.Models;

namespace NewSpotify.Web.Services
{
    public class ModelConverterService
    {
        public IndexVm ConvertToIndexVm(SearchCategoriesResponse response, List<SelectedSongItem> Selections)
        {
            var indexVm = new IndexVm();
            indexVm.Categories = response.Categories.Items
        }
    }
}
