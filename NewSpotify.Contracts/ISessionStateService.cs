using NewSpotify.Models.Models.StateManagerModels;
using System;
using System.Collections.Generic;

namespace NewSpotify.Contracts
{
    public interface ISessionStateService
    {
        List<SelectedSongItem> GetLikedSongs();
        void SetLikedSongs(string trackId, string songName, string imageUrl, string bandName);
    }
}
