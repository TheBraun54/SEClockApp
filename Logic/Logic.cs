namespace SEClockApp.Logic;

public class Logic : ILogic
{
	public Logic()
	{
		
	}

    public static class Directories
    {
        public static List<string> SelectedDirectories = new List<string>();
        public static string AudioDirPath, AudioFileName, AudioFilePath;

        /// <summary>
        /// Gets a list of audio files in the given directory
        /// </summary>
        /// <param name="AudioDirPath"></param>
        /// <returns> List<string> </returns>
        public static string[] getAudioFiles(string AudioDirPath)
        {
            DirectoryInfo AudioDir = new DirectoryInfo(AudioDirPath);
            FileInfo[] Files = AudioDir.GetFiles();
            List<string> AudioFiles = new List<string>();
            foreach (FileInfo File in Files)
            {
                // add to AudioFiles if it ends with .mp3
                if (File.Name.Length >= 4)
                {
                    if (File.Name.EndsWith("mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        //System.Diagnostics.Debug.WriteLine("Audio: " + File.Name);
                        AudioFiles.Add(File.Name);
                    }
                }
            }
            return AudioFiles.ToArray();
        }

        /// <summary>
        /// Selects a random song from a random SelectedDirectory
        /// and returns its path as a string
        /// </summary>
        public static string GetRandomPath()
        {
            Random rand = new Random();
            if (SelectedDirectories.Count() > 0)
            {
                AudioDirPath = SelectedDirectories[rand.Next(0, SelectedDirectories.Count())];// random dir
                //System.Diagnostics.Debug.WriteLine("AudioDirPath: " + AudioDirPath);
                string[] AudioFiles = Directories.getAudioFiles(AudioDirPath);
                if (AudioFiles.Length > 0)
                {
                    AudioFileName = AudioFiles[rand.Next(0, AudioFiles.Length)];// random song in dir
                    AudioFilePath = Path.Combine(AudioDirPath, AudioFileName);
                    return AudioFilePath;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Music not found");
                    // notify user
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No directories avalible");
                // notify user
            }
            return null;
        }
    }

    public static class PlaylistGenerator
    {
        /// <summary>
        /// Creates a playlist using random songs
        /// </summary>
        /// <returns></returns>
        public static Playlist GetRandomPlaylist()
        {
            Playlist RandomPlaylist = new Playlist("Random", 5);
            List<string> Paths = new List<string>();
            for (int i = 0; i < RandomPlaylist.Length; i++)
            {
                Paths.Add(Directories.GetRandomPath());
            }
            RandomPlaylist.Songs = Paths;
            return RandomPlaylist;
        }
    }
}