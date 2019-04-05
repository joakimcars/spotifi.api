using Newtonsoft.Json;

namespace NewSpotify.Models.Models.Spotify
{
    public class SpotifyItem
    {

        [JsonProperty("added_at")]
        public System.DateTime AddedAt { get; set; }

        [JsonProperty("added_by")]
        public SpotifyAddedBy AddedBy { get; set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }

        [JsonProperty("primary_color")]
        public string PrimaryColor { get; set; }

        [JsonProperty("track")]
        public SpotifyTrack Track { get; set; }

        [JsonProperty("video_thumbnail")]
        public SpotifyVideoThumbnail VideoThumbnail { get; set; }
    }
}