using Newtonsoft.Json;

namespace NewSpotify.Models.Models.StateManagerModels
{
    public class SelectedSongItem
    {
        [JsonProperty("TrackId")]
        public string TrackId { get; set; }

        [JsonProperty("SongName")]
        public string SongName { get; set; }

        [JsonProperty("BandName")]
        public string BandName { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }
    }
}