using NAudio.Wave;

namespace SEClockApp.Logic;

// Primary Author: Zach La Vake
// Secondary Author: Brady Braun
// Reviewer: Paul Hwang
public class Logic : ILogic
{
    public Logic()
    {

    }

    public static class Directories
    {
        public static List<string> SelectedDirectories = new List<string>();
        public static List<Song> SongList = new List<Song>();
        public static string AudioDirPath, AudioFileName, AudioFilePath;

        /// <summary>
        /// Adds a directory to SelectedDirectories then updates the SongList
        /// </summary>
        /// <param name="Path"></param>
        public static void AddDirectory(string Path)
        {
            SelectedDirectories.Add(Path);
            UpdateSongList();
        }

        /// <summary>
        /// Removes a directory from SelectedDirectories then updates the SongList
        /// </summary>
        /// <param name="Path"></param>
        public static void RemoveDirectory(string Path)
        {
            SelectedDirectories.Remove(Path);
            UpdateSongList();
        }

        /// <summary>
        /// Updates SongList 
        /// </summary>
        public static void UpdateSongList()
        {
            SongList = new List<Song>();
            foreach (string Dir in SelectedDirectories)
            {
                foreach (string AudioPath in getAudioPaths(Dir))
                {
                    SongList.Add(new Song(AudioPath, new AudioFileReader(AudioPath).TotalTime));
                }
            }
        }

        /// <summary>
        /// Gets a list of audio file paths in the given directory
        /// </summary>
        /// <param name="AudioDirPath"></param>
        /// <returns> List </returns>
        public static List<string> getAudioPaths(string AudioDirPath)
        {
            DirectoryInfo AudioDir = new DirectoryInfo(AudioDirPath);
            FileInfo[] Files = AudioDir.GetFiles();
            List<string> AudioFilePaths = new List<string>();
            foreach (FileInfo File in Files)
            {
                // add to AudioFiles if it ends with .mp3
                if (File.Name.Length >= 4)
                {
                    if (File.Name.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        //System.Diagnostics.Debug.WriteLine("Audio: " + File.Name);
                        AudioFilePath = Path.Combine(AudioDirPath, File.Name);
                        AudioFilePaths.Add(AudioFilePath);
                    }
                }
            }
            return AudioFilePaths;
        }

       
        /// <summary>
        /// Selects a random song from a random SelectedDirectory
        /// </summary>
        public static Song GetRandomSong()
        {
            if (SongList.Count() > 0)
            {
                return SongList[new Random().Next(0, SongList.Count)];
            }
            else return null;
        }

        /// <summary>
        /// Prints the SelectedDirectories
        /// </summary>
        /// <returns></returns>
        public static void PrintDirectories()
        {
            foreach (string s in SelectedDirectories)
            {
                System.Diagnostics.Debug.WriteLine(s);
            }
        }
    }

    public static class PlaylistGenerator
    {
        /// <summary>
        /// Creates a playlist with a certain number of song and a random duration
        /// </summary>
        /// <returns>Playlist</returns>
        public static Playlist GetPlaylist(int songCount)
        {
            List<Song> Songs = new List<Song>();
            TimeSpan Duration = new TimeSpan(0,0,0);
            for (int i = 0; i < songCount; i++)
            {
                Song Song = Directories.GetRandomSong();
                if (Song != null)
                {
                    Songs.Add(Song);
                    Duration = Duration.Add(Song.Duration);
                }
            }
            return new Playlist("Random", Duration, Songs);
        }

        /// <summary>
        /// Creates a playlist which gets close to the given duration using random songs 
        /// </summary>
        /// <returns>Playlist</returns>
        public static Playlist GetPlaylist(TimeSpan RequestedDuration)
        {
            List<Song> Songs = new List<Song>();
            TimeSpan Duration = new TimeSpan(0, 0, 0);
            Song Song = Directories.GetRandomSong();
            if (Song == null)
            {
                return null;
            }
            TimeSpan DurationWithSong = Duration.Add(Song.Duration); ;
            while (DurationWithSong.TotalSeconds <= RequestedDuration.TotalSeconds) 
            {
                Songs.Add(Song);
                Duration = DurationWithSong;
                Song = Directories.GetRandomSong();
                DurationWithSong = Duration.Add(Song.Duration);
            }
            return new Playlist("GetPlaylist", Duration, Songs);
        }

        /// <summary>
        /// Creates a playlist which lasts exactly the given duration using random songs 
        /// </summary>
        /// <returns>Playlist</returns>
        public static Playlist GetPlaylistV2(TimeSpan RequestedDuration)
        {
            List<Song> Songs = new List<Song>();

            // todo: get as close as possible using a delay between songs
            // Playlist.Duration
            //return new Playlist("GetPlaylist", Duration, Songs);
            return GetPlaylist(5);
        }
    }

    /// <summary>
    /// Verifies that the Spotify settings are all set before trying to play music from it
    /// </summary>
    /// <returns>List<String> containing proper error messages, an array with one empty string otherwise</returns>
    public static List<String> SpotifyLogic()
    {
        List<String> messages = new List<String>();

        if (MauiProgram.spotify == null) // User has not connected their Spotify account yet
        {
            messages.Add("Connect your Spotify in Settings");
            messages.Add("or toggle for the timer to play local music in Settings");
        }
        else if (MauiProgram.playlistId == "empty") // User has not selected a playlist
        {
            messages.Add("Select a playlist!");
            messages.Add("Go to the Spotify tab below");
        }
        else // User has connected their Spotify and selected a playlist, no issues found
        {
            messages.Add("");
        }
        return messages;
    }
}