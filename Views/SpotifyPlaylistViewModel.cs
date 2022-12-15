using System.Collections.Generic;
using System.Collections.ObjectModel;

// Primary Author: Paul Hwang
// Secondary Author: Brady Braun
// Reviewer: Brady Braun

namespace SEClockApp
{
    public class SpotifyPlaylistViewModel
    {
        // Class is used to dispaly the games in the ListView of the MainPage.xaml
        public ObservableCollection<SpotifyPlaylist> SpotifyPlaylists { get; private set; }

        /// <summary>
        /// Constructor for a GamesViewModel object that creates the ObservableCollection of Games
        /// </summary>
        public SpotifyPlaylistViewModel()
        {
            SpotifyPlaylists = new ObservableCollection<SpotifyPlaylist>();
        }

        /// <summary>
        /// Adds a playlist's name and image to the ObservableCollections
        /// </summary>
        /// <param name="playlistToBeAdded">Playlist to be added from a Spotify account</param>
        public bool AddPlaylist(SpotifyPlaylist playlistToBeAdded)
        {
            try
            {
                SpotifyPlaylists.Add(playlistToBeAdded);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ObservableCollection<SpotifyPlaylist> GetPlaylists()
        {
            return SpotifyPlaylists;
        }
    }
}