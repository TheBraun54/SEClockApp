using CommunityToolkit.Mvvm.ComponentModel;
using NAudio.Wave;
using static SEClockApp.Logic.Logic;

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
        string title;
        TimeSpan duration;
        TimeSpan delay = new TimeSpan(0,0,1); 
        List<Song> songs = new List<Song>();

        /// <summary>
        /// Constructor for a Playlist
        /// </summary>
        /// <param name="title"></param>
        /// <param name="duration"></param>
        public Playlist(string title, TimeSpan duration, List<Song> songs)
        {
            this.title = title;
            this.duration = duration;
            this.songs = songs;
        }

        public String Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set { SetProperty(ref duration, value); }
        }

        // time to wait between songs in order to make it the correct duration
        public TimeSpan Delay
        {
            get { return delay; }
            set { SetProperty(ref delay, value); }
        }
        
        public List<Song> Songs
        {
            get { return songs; }
            set { SetProperty(ref songs, value); }
        }

        /// <summary>
        /// For debugging purposes: prints all the songs in the playlist
        /// </summary>
        public void PrintPlaylist()
        {
            System.Diagnostics.Debug.WriteLine("\nCurrent Playlist: " + title);
            foreach (Song song in songs)
            {
                System.Diagnostics.Debug.WriteLine(song.Path + "    Duration: " + song.Duration);
            }
            System.Diagnostics.Debug.WriteLine("Total duration: " + duration + "\n");
        }
    }

    /// <summary>
    /// Represents a song
    /// </summary>
    public class Song
    {
        public string Path;
        public TimeSpan Duration;
        public Song(string Path, TimeSpan Duration)
        {
            this.Path = Path;
            this.Duration = Duration;
        }
    }
}