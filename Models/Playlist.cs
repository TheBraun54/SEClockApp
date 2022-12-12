using CommunityToolkit.Mvvm.ComponentModel;

// Primary Author: Zach La Vake
// Secondary Author: Brady Braun
// Reviewer: Paul Hwang

namespace SEClockApp
{
    /// <summary>
    /// Represents a playlist
    /// </summary>
    public class Playlist : ObservableObject
    {
        int length;
        string title;
        string imageUrl;
        string playlistId;
        List<string> songs;

        /// <summary>
        /// Constructor for a playlist
        /// </summary>
        /// <param name="title">title of the playlist</param>
        /// <param name="length">length of the playlist (in mins)</param>
        public Playlist(string title, int length)
        {
            this.title = title;
            this.length = length;
        }
        /// <summary>
        /// Constructor for a playlist, used when creating playlists acquired from Spotify
        /// </summary>
        /// <param name="title">title of the playlist</param>
        /// <param name="length">length of the playlist (in mins)</param>
        /// <param name="imageUrl">imageUrl of a playlist from spotify (used to display playlist cover art)</param>
        /// <param name="playlistId">specific id of the playlist from spotify</param>
        public Playlist(string title, int length, string imageUrl, string playlistId)
        {
            this.title = title;
            this.length = length;
            this.imageUrl = imageUrl;
            this.playlistId = playlistId;
        }

        public String Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public int Length
        {
            get { return length; }
            set { SetProperty(ref length, value); }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value); }
        }

        public string PlaylistId
        {
            get { return playlistId; }
            set { SetProperty(ref playlistId, value); }
        }

        public List<string> Songs
        {
            get { return songs; }
            set { SetProperty(ref songs, value); }
        }

        /// <summary>
        /// For debugging purposes: prints all the songs in the playlist
        /// </summary>
        public void PrintPlaylist()
        {
            System.Diagnostics.Debug.WriteLine("\nCurrent Playlist: ");
            foreach (string song in songs)
            {
                System.Diagnostics.Debug.WriteLine(song);
            }
            System.Diagnostics.Debug.WriteLine("\n");
        }
    }
}