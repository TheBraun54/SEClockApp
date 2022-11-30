using Microsoft.Maui.Layouts;
using NAudio.Wave;
using static SEClockApp.Logic.Logic;

namespace SEClockApp;
/*
 * Primary Author: Brady Braun
 * Secondary Author: Zach La Vake
 * Reviewer: 
 */

public partial class MainPage : ContentPage
{
    private TimeOnly time = new TimeOnly(01, 30, 00);

    private bool isRunning;

    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public string AudioFilePath;
    public int SongIndex = 0;

    Playlist CurrentPlaylist;
    List<string> CurrentSongs;
    public MainPage()
    {
        InitializeComponent();
        
    }

    private async void SettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"Settings");
    }

    private async void MusicClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"Spotify");
    }

    private void StartClock(object sender, EventArgs e)
    {
        Main.IsVisible = false;
        Player.IsVisible = true;

        // Clock
        isRunning = true;
        Clock();

        // Audio
        CurrentPlaylist = PlaylistGenerator.GetRandomPlaylist();
        CurrentSongs = CurrentPlaylist.Songs;
        CurrentPlaylist.PrintPlaylist();

        if (CurrentSongs.Count > 0)
        {
            AudioFilePath = CurrentSongs[SongIndex];
            Play(AudioFilePath);
        }
    }


    public void AlarmTimer(object sender, EventArgs e)
    {
        if (AlarmTimerSwitch.IsToggled == true)
        {
            Timer.Format = "t";
        }
        else
        {
            Timer.Format = "hh:mm:ss";
        }
    }

    /// <summary>
    /// Changes Display every second
    /// </summary>
    public async void Clock()
    {
        while (isRunning)
        {
            time = time.Add(TimeSpan.FromSeconds(-1));
            Display.Text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00}";
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

            outputDevice?.Pause();
        }
    }

    /// <summary>
    /// Sets outputDevice and audioFile if needed then plays audio
    /// </summary>
    /// <param name="FilePath"></param>
    public void Play(string FilePath)
    {
        if (FilePath != null)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += MusicStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(FilePath);
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("null FilePath in Play");
        }

    }

    /// <summary>
    /// Called when audio stops, plays next song if timer is running
    /// </summary>
    private void MusicStopped(object sender, StoppedEventArgs args)
    {
        // remove old audio 
        if (outputDevice != null)
        {
            outputDevice.Dispose();
            outputDevice = null;
        }
        if (audioFile != null)
        {
            audioFile.Dispose();
            audioFile = null;
        }

        // start new audio
        if (isRunning && SongIndex + 1 < CurrentSongs.Count)
        {
            SongIndex++;
            Play(CurrentSongs[SongIndex]);
        }
    }

    /// <summary>
    /// Handler for the Stop button
    /// Stops audio and goes back a page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void StopButtonHandler(object sender, EventArgs e)
    {
        Player.IsVisible = false;
        Main.IsVisible = true;
        isRunning = false;
        outputDevice?.Stop();
    }

}

