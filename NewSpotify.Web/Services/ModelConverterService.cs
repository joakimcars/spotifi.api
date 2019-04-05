using System.Collections.Generic;
using System.Linq;
using NewSpotify.Web.Models;
using NewSpotify.Web.Models.Spotify;
using NewSpotify.Web.Models.ViewModels;

namespace NewSpotify.Web.Services
{
    public class ModelConverterService
    {
        public IndexVm ConvertToIndexVm(SpotifySearchCategoriesResponse response, List<SelectedSongItem> selections)
        {
            return new IndexVm
            {
                Categories = BuildCategoryViewModels(response.Categories.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections)  
            };
        }

        private static List<SelectedSongsVm> BuildSelectedSongsViewModels(List<SelectedSongItem> selectedSongItems)
        {
            var newList = selectedSongItems.Select(s => new SelectedSongsVm
            {
                SongName = s.SongName,
                BandName = s.BandName,
                ImageUrl = s.ImageUrl,
                TrackId = s.TrackId
            });
            return newList.ToList();
        }

        private static IList<CategoryVm> BuildCategoryViewModels(IList<SpotifyCategories> categories)
        {
            var newList = categories.Select(s => new CategoryVm
            {
                Name = s.Name,
                Id = s.Id,
                ImageUrl = s.Icons[0].Url
            });
            return newList.ToList();
        }

        public PlaylistsVm ConvertToPlaylistVm(SpotifySearchPlayListResponse response, List<SelectedSongItem> selections)
        {
            return new PlaylistsVm()
            {
                PlayLists = BuildPlaylistsViewModels(response.Playlists.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections)
            };
        }

        private static List<PlayListVm> BuildPlaylistsViewModels(IList<SpotifyPlaylists> playLists)
        {
            var newList = playLists.Select(s => new PlayListVm
            {
                Name = s.Name,
                Id = s.Id,
                ImageUrl = s.Images[0].Url
            });
            return newList.ToList();
        }

        public TracksVm ConvertToTracksVm(SpotifyTrackresponse response, List<SelectedSongItem> selections, string playListId ="")
        {
            return new TracksVm()
            {
                Tracks = BuildTracksViewModels(response.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections),
                PlaylistId = playListId
            };
        }

        public TracksVm ConvertToTracksVm(SpotifySearchTrackResponse response, List<SelectedSongItem> selections, string playlistId="")
        {
            return new TracksVm()
            {
                Tracks = BuildTracksViewModels(response.Tracks.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections),
                PlaylistId = playlistId
            };
        }



        private static List<TrackVm> BuildTracksViewModels(IList<SpotifyItem> tracks)
        {
            var newList = tracks.Select(s => new TrackVm()
            {
                Name = s.Track.Name,
                Id = s.Track.Id,
                ImageUrl = s.Track.Album.Images[0].Url,
                ArtistName = s.Track.Artists[0].Name,
                AlbumName = s.Track.Album.Name,
                Popularity = s.Track.Popularity
            });
            return newList.ToList();
        }

        private static List<TrackVm> BuildTracksViewModels(IList<SpotifyTrack> tracks)
        {
            var newList = tracks.Select(s => new TrackVm()
            {
                Name = s.Name,
                Id = s.Id,
                ImageUrl = s.Album.Images[0].Url,
                ArtistName = s.Artists[0].Name,
                AlbumName = s.Album.Name,
                Popularity = s.Popularity
            });
            return newList.ToList();
        }

        private static List<RecommendationVm> BuildRecommendationsViewModels(IList<SpotifyTrack> tracks)
        {
            var newList = tracks.Select(s => new RecommendationVm()
            {
                ImageUrl = s.Album.Images.FirstOrDefault()?.Url,
                Name = s.Name,
                AlbumName = s.Album.Name,
                ArtistName = s.Artists[0].Name,
                Popularity = s.Popularity,
                SpotifyUrl = s.Uri
            });
            return newList.ToList();
        }

        public RecommendationsVm ConvertToRecommendationsVm(List<SpotifyTrack> response)
        {

            return new RecommendationsVm
            {
                Recommendations = BuildRecommendationsViewModels(response)
            };
        }
    }
}
