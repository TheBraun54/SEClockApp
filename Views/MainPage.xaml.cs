using NAudio.Wave;
using static SEClockApp.Logic.Logic;

namespace SEClockApp;
/*
 * Primary Author: Brady 
 * Secondary Author: Zach & Qadar
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

    Playlist CurrentPlaylist;
    List<string> CurrentSongs;

    public MainPage()
    {
        InitializeComponent();
    }

    private void StartClock(object sender, EventArgs e)
    {
        Main.IsVisible = false;
        Player.IsVisible = true;
        isRunning = true;
        TimeSpan alarmTime = Alarm.Time - DateTime.Now.TimeOfDay;
        if (AlarmTimerSwitch.IsToggled)
        {
            time = new TimeOnly(alarmTime.Hours, alarmTime.Minutes, alarmTime.Seconds);
        }
        else
        {
            time = new TimeOnly(hours, minutes, seconds);
        }


        TimerClock();
    }

    public async void TimerClock()
    {
        // Start the clock
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

    public void Reset()
    {
        // Reset the labels
        Hours.Text = "00";
        Minutes.Text = "00";
        Seconds.Text = "00";

        // Reset time
        time = new TimeOnly(0, 0, 0);
        hours = 0;
        minutes = 0;
        seconds = 0;

        // Reset slider values
        HrSlider.Value = 0;
        MinSlider.Value = 0;
        SecSlider.Value = 0;
    }

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
        while (isRunning)
        {
            time = time.Add(TimeSpan.FromSeconds(-1));
            Display.Text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00}";
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Stops the timer when our timer is over
            if (time.Hour == 0 && time.Minute == 0 && time.Second == 0)
            {
                isRunning = !isRunning;
                Main.IsVisible = true;
                Player.IsVisible = false;
                Reset();
            }
        }
    }

    /// <summary>
    /// Handler for the play and pause button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public async void PlayPauseHandler(object sender, EventArgs e)
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
}

