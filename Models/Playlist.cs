using CommunityToolkit.Mvvm.ComponentModel;

namespace SEClockApp
{
    /// <summary>
    /// Represents a playlist
    /// </summary>
    public class Playlist : ObservableObject
    {
        int length;
        string title;
        List<string> songs;
        public Playlist(string title, int length)
        {
            this.title = title;
            this.length = length;

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
        
        public List<string> Songs
        {
            get { return songs; }
            set { SetProperty(ref songs, value); }
        }

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