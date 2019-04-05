using System.Collections.Generic;
using NewSpotify.Models.Models.StateManagerModels;
using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifySearchPlayListResponse
    {
        public List<SelectedSongItem> SelectedSongs { get; set; }

        [JsonProperty("playlists")]
        public SpotifySearchPlayListsCollection Playlists { get; set; }
    }
}
