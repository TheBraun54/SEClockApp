using NAudio.Wave;
using static SEClockApp.Logic.Logic;
using static SEClockApp.SpotifyPlaylist;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using Device = SpotifyAPI.Web.Device;

namespace SEClockApp;
/*
 * Primary Author: Brady (Main Page, Timer, Alarm Clock Screen) and Zach (audio) 
 * Secondary Author: 
 * Reviewer: Paul 
 */

public partial class MainPage : ContentPage
{
    private int hours;
    private int minutes;
    private int seconds;
    private TimeOnly time;

    private bool isRunning;

    private WaveOutEvent outputDevice;
    private AudioFileReader audioFile;

    public string AudioFilePath;
    public int SongIndex = 0;
    Boolean TimerMode = false;

    Playlist CurrentPlaylist;
    List<Song> CurrentSongs;

    public MainPage()
    {
        InitializeComponent();
        Directories.UpdateSongList();
    }

    /// <summary>
    /// Starts the clock by making the timer visible
    /// Also starts playing music
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void StartClock(object sender, EventArgs e)
    {
        // Audio
        if (MauiProgram.isSpotify) // Play music from the user's Spotify account
        {
            // List will have proper display error messages
            // a "" if no issues were found and we can proceed to play music
            List<String> displayAlertMessages = SpotifyLogic();
            
            if (displayAlertMessages.ElementAt(0) != "") // Issues were found
            {
                await DisplayAlert(displayAlertMessages.ElementAt(0),
                    displayAlertMessages.ElementAt(1),
                    "ok");
                return;     // prevents the timer from starting and playing music
            }
            else // No issues were found, start playing music on Spotify
            {
                try
                {
                    PlaySpotify(new TimeSpan(hours, minutes, seconds));
                }
                catch(APIException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{ex.GetType()}");
                    await DisplayAlert("Something went wrong", "Make sure you have an active device ready with Spotify open", "ok");
                    return; // prevents the timer from starting
                }
            }
        }
        else // Play music from the local device
        {
            TimerMode = AlarmTimerSwitch.IsToggled;
            if (!TimerMode)
            {
                TimerMode = false;
                CurrentPlaylist = PlaylistGenerator.GetPlaylistV2(new TimeSpan(hours, minutes, seconds));
                if (CurrentPlaylist != null)
                {
                    CurrentSongs = CurrentPlaylist.Songs;
                    CurrentPlaylist.PrintPlaylist();
                    if (CurrentSongs.Count > 0)
                    {
                        AudioFilePath = CurrentSongs[SongIndex].Path;
                        Play(AudioFilePath);
                    }
                }
            }
            else // Alarm is toggled instead
            {
                TimeSpan alarmTime = Alarm.Time.Subtract(DateTime.Now.TimeOfDay);
                if (alarmTime.Hours < 0 || alarmTime.Minutes < 0 || alarmTime.Seconds < 0)
                {
                    time = new TimeOnly(Math.Abs(23 + alarmTime.Hours), Math.Abs(60 + alarmTime.Minutes), Math.Abs(alarmTime.Seconds));
                }
                else
                {
                    time = new TimeOnly(alarmTime.Hours, alarmTime.Minutes, alarmTime.Seconds);
                }
            }
        }

        // Displays the timer
        Main.IsVisible = false;
        Player.IsVisible = true;
        isRunning = true;
        time = new TimeOnly(hours, minutes, seconds);

        // Clock
        isRunning = true;
        PlayPauseButton.BorderColor = Color.FromArgb("#F1E3F3");
        DisplayBorder.Stroke = Color.FromArgb("#F1E3F3");
        PlayPauseButton.Text = "II";
        Clock();
    }

    /// <summary>
    /// Resets the timer text values to 0, also resets the time variable
    /// as well as the slider values for defining a timer length
    /// </summary>
    public void Reset()
    {
        Hours.Text = "00";
        Minutes.Text = "00";
        Seconds.Text = "00";
        HrSlider.Value = 0;
        MinSlider.Value = 0;
        SecSlider.Value = 0;

        if (MauiProgram.isSpotify)
        {
            MauiProgram.spotify.Player.PausePlayback();
        }
        else
        {
            outputDevice?.Stop();
        }
    }

    /// <summary>
    /// Determines whether the user wants to use the timer or alarm functionality
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void AlarmTimer(object sender, EventArgs e) { 
        if (AlarmTimerSwitch.IsToggled == true)
        {
            Alarm.IsVisible = true;
            Timer.IsVisible = false;
        }
        else
        {
            Alarm.IsVisible = false;
            Timer.IsVisible = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnHourChanged(object sender, ValueChangedEventArgs args)
    {
        int value = (int)args.NewValue;
        hours = value;
        if (value - 10 < 0)
        {
            Hours.Text = String.Format("0{0}", value);
        }
        else
        {
            Hours.Text = String.Format("{0}", value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnMinuteChanged(object sender, ValueChangedEventArgs args)
    {
        int value = (int)args.NewValue;
        minutes = value;
        if (value - 10 < 0)
        {
            Minutes.Text = String.Format("0{0}", value);
        }
        else
        {
            Minutes.Text = String.Format("{0}", value);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public void OnSecondChanged(object sender, ValueChangedEventArgs args)
    {
        int value = (int)args.NewValue;
        seconds = value;
        if (value - 10 < 0)
        {
            Seconds.Text = String.Format("0{0}", value);
        }
        else
        {
            Seconds.Text = String.Format("{0}", value);
        }

    }


    /// <summary>
    /// Changes Display every second
    /// </summary>
    public async void Clock()
    {
        while (Player.IsVisible)
        {
            while (isRunning)
            {
                time = time.Add(TimeSpan.FromSeconds(-1));
                Display.Text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00}";
                await Task.Delay(TimeSpan.FromSeconds(1));
                if (time.Hour == 0 && time.Minute == 0 && time.Second == 0)
                {
                    isRunning = !isRunning;              
                    if (TimerMode)
                    {
                        Song timerSong = Directories.GetRandomSong();
                        if (timerSong != null)
                        {
                            Play(timerSong.Path);
                        }
                    } else
                    {
                        Main.IsVisible = true;
                        Player.IsVisible = false;
                        Reset();
                    }
                }
                if (!Player.IsVisible)
                {
                    break;
                }
            }
            // paused
            await Task.Delay(TimeSpan.FromSeconds(0.5));
        }

        //System.Diagnostics.Debug.WriteLine("Clock stopped");
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
            PlayPauseButton.BorderColor = Color.FromArgb("#F1E3F3");
            DisplayBorder.Stroke = Color.FromArgb("#F1E3F3");

            if (MauiProgram.isSpotify)
            {
                MauiProgram.spotify.Player.ResumePlayback();
            }
            else
            {
                Play(AudioFilePath);
            }
        }
        else
        {
            PlayPauseButton.BorderColor = Color.FromArgb("#62BFED");
            DisplayBorder.Stroke = Color.FromArgb("#62BFED");

            if (MauiProgram.isSpotify)
            {
                MauiProgram.spotify.Player.PausePlayback();
            }
            else
            {
                outputDevice?.Pause();
            }
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
        
        // delay between songs, currently causes stop button bug
        // Task.Delay(CurrentSongs[SongIndex].Delay);

        // start new audio
        if (isRunning && SongIndex + 1 < CurrentSongs.Count)
        {
            SongIndex++;
            Play(CurrentSongs[SongIndex].Path);
        }
    }

    /// <summary>
    /// Handler for the Stop button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void StopButtonHandler(object sender, EventArgs e)
    {
        isRunning = false;
        Player.IsVisible = false;
        Main.IsVisible = true;
        Reset();
    }
}