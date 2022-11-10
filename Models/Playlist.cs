using CommunityToolkit.Mvvm.ComponentModel;

namespace SEClockApp
{
    /// <summary>
    /// Representing a potential playlist that our application would generate
    /// </summary>
    public class Playlist : ObservableObject
    {
        int length;
        string title;
        public Playlist(string title, int length)
        {
            // Dummy constructor as of now to have an object
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
    }
}
