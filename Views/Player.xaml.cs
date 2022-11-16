using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SEClockApp;

public partial class Player : ContentPage
{
    private TimeOnly time = new TimeOnly(00, 10, 00);

    private bool isRunning;

    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public string AudioDir, AudioFile, AudioFilePath;

    public Player()
    {
        InitializeComponent();

        // Clock
        isRunning = true;
        Clock();

        // Audio
        AudioDir = @"D:\";// from music select page
        AudioFile = @"test.mp3";// from scanning files in AudioDir
        AudioFilePath = Path.Combine(AudioDir, AudioFile);
        Play();
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

            Play();
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
    public void Play()
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            audioFile = new AudioFileReader(AudioFilePath);
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