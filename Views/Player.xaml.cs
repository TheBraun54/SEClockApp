using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SEClockApp;

public partial class Player : ContentPage
{
    private TimeOnly time = new TimeOnly(00, 10, 00);

    private bool isRunning;

    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public string AudioDirPath, AudioFileName, AudioFilePath;

    public Player()
    {
        InitializeComponent();

        // Clock
        isRunning = true;
        Clock();

        // Audio
        Random rand = new Random();
        List<string> Dirs = Directories.SelectedDirectories;
        if (Dirs.Count() > 0)
        {
            AudioDirPath = Dirs[rand.Next(0, Dirs.Count())];// random dir
            System.Diagnostics.Debug.WriteLine("AudioDirPath: " + AudioDirPath);
            string[] AudioFiles = getAudioFiles(AudioDirPath);
            if (AudioFiles.Length > 0)
            {
                AudioFileName = AudioFiles[rand.Next(0, AudioFiles.Length)];// random song in dir
                AudioFilePath = Path.Combine(AudioDirPath, AudioFileName);
                Play(AudioFilePath);
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
    }

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
    /// Increments Display every second
    /// </summary>
    public async void Clock()
    {
        while (isRunning)
        {
            time = time.Add(TimeSpan.FromSeconds(-1));
            Display.Text = $"{time.Minute:00}:{time.Second:00}";
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    /// <summary>
    /// Handler for the play and pause button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void PlayPauseHandler(object sender, EventArgs e)
    {
        isRunning = !isRunning;
        PlayPauseButton.Text = isRunning ? "II" : "\u25BA";
        if (isRunning)
        {
            Clock();
            PlayPauseButton.BorderColor = Color.FromArgb("#F1E3F3");
            DisplayBorder.Stroke = Color.FromArgb("#F1E3F3");

            Play(AudioFilePath);
        }
        else
        {
            PlayPauseButton.BorderColor = Color.FromArgb("#62BFED");
            DisplayBorder.Stroke = Color.FromArgb("#62BFED");

            outputDevice?.Stop();
        }
    }

    /// <summary>
    /// Sets outputDevice and audioFile if needed then plays audio
    /// </summary>
    /// <param name="FilePath"></param>
    public void Play(string FilePath)
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(FilePath);
            outputDevice.Init(audioFile);
        }
        outputDevice.Play();
    }


    /// <summary>
    /// Removes audio data after outputDevice.PlaybackStopped
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        audioFile.Dispose();
        audioFile = null;
    }

    /// <summary>
    /// Handler for the Stop button
    /// Stops audio and goes back a page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void StopButtonHandler(object sender, EventArgs e)
    {
        outputDevice?.Stop();
        await Shell.Current.GoToAsync("..");
    }
}