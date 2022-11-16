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
        AudioDirPath = @"D:\";// todo: get from select page
        string[] AudioFiles = getAudioFiles(AudioDirPath);

        // get AudioFilePath for a random song in AudioDirPath
        Random rand = new Random();
        AudioFileName = AudioFiles[rand.Next(0, AudioFiles.Length)];
        AudioFilePath = Path.Combine(AudioDirPath, AudioFileName);

        Play(AudioFilePath);
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
            System.Diagnostics.Debug.WriteLine(File.Name);
            if (File.Name.Length >= 4)
            {
                if (File.Name.Substring(File.Name.Length - 4, 4).Equals(".mp3"))
                {
                    System.Diagnostics.Debug.WriteLine("Audio: " + File.Name);
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