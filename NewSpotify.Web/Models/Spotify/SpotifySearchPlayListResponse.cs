using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NewSpotify.Web.Models.Spotify
{
    public class SpotifySearchPlayListResponse
    {
        public List<SelectedSongItem> SelectedSongs { get; set; }

        [JsonProperty("playlists")]
        public SpotifySearchPlayListsCollection Playlists { get; set; }
    }
}
