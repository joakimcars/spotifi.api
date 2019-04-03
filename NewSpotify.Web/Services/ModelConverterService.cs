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

        private List<SelectedSongsVm> BuildSelectedSongsViewModels(List<SelectedSongItem> selectedSongItems)
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

        private IList<CategoryVm> BuildCategoryViewModels(IList<SpotifyCategories> categories)
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

        private List<PlayListVm> BuildPlaylistsViewModels(IList<SpotifyPlaylists> playLists)
        {
            var newList = playLists.Select(s => new PlayListVm
            {
                Name = s.Name,
                Id = s.Id,
                ImageUrl = s.Images[0].Url
            });
            return newList.ToList();
        }

        //public PlayListVm BuildPlayListVm(SpotifyPlaylists playlist)
        //{
        //    var newPlaylist = new PlayListVm
        //    {
        //        Name = playlist.Name,
        //        Id = playlist.Id,
        //        ImageUrl = playlist.Images[0].Url
        //    };

        //    return newPlaylist;
        //}

        public TracksVm ConvertToTracksVm(SpotifyTrackresponse response, List<SelectedSongItem> selections)
        {
            return new TracksVm()
            {
                Tracks = BuildTracksViewModels(response.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections)
            };
        }

        public TracksVm ConvertToTracksVm(SpotifySearchTrackResponse response, List<SelectedSongItem> selections)
        {
            return new TracksVm()
            {
                Tracks = BuildTracksViewModels(response.Tracks.Items),
                SelectedSongs = BuildSelectedSongsViewModels(selections)
            };
        }



        private List<TrackVm> BuildTracksViewModels(IList<SpotifyItem> tracks)
        {

            var newList = new List<TrackVm>();
            foreach (var track in tracks)
            {
                newList.Add(BuildTrackVm(track));
            }
            return newList;
        }

        private List<TrackVm> BuildTracksViewModels(IList<SpotifyTrack> tracks)
        {

            var newList = new List<TrackVm>();
            foreach (var track in tracks)
            {
                newList.Add(BuildTrackVm(track));
            }
            return newList;
        }



        public TrackVm BuildTrackVm(SpotifyTrack track)
        {
            var newTrackVm = new TrackVm
            {
                Name = track.Name,
                Id = track.Id,
                ImageUrl = track.Album.Images[0].Url,
                ArtistName = track.Artists[0].Name,
                AlbumName = track.Album.Name,
                Popularity = track.Popularity
            };

            return newTrackVm;
        }

        public TrackVm BuildTrackVm(SpotifyItem track)
        {
            var newTrackVm = new TrackVm
            {
                Name = track.Track.Name,
                Id = track.Track.Id,
                ImageUrl = track.Track.Album.Images[0].Url,
                ArtistName = track.Track.Artists[0].Name,
                AlbumName = track.Track.Album.Name,
                Popularity = track.Track.Popularity
            };

            return newTrackVm;
        }

        private List<RecommendationVm> BuildRecommendationsViewModels(IList<SpotifyTrack> tracks)
        {

            var newList = new List<RecommendationVm>();
            foreach (var track in tracks)
            {
                newList.Add(BuildRecommendationVm(track));
            }
            return newList;
        }

        public RecommendationVm BuildRecommendationVm(SpotifyTrack track)
        {
            var recommendationVm = new RecommendationVm
            {
                ImageUrl = track.Album.Images.FirstOrDefault()?.Url,
                Name = track.Name,
                AlbumName = track.Album.Name,
                ArtistName = track.Artists[0].Name,
                Popularity = track.Popularity,
                SpotifyUrl = track.Uri
            };

            return recommendationVm;

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
