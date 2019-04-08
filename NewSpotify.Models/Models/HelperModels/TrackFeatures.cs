using System;
using System.Collections.Generic;
using System.Text;

namespace NewSpotify.Models.Models.HelperModels
{
    public class TrackFeatures
    {
        public double Danceability { get; set; }

        public double Acousticness { get; set; }

        public double Energy { get; set; }

        public double Instrumentalness { get; set; }

        public List<string> Tracks { get; set; }
    }
}
