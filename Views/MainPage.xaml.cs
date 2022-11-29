using Android.Views;
using Microsoft.Maui.Layouts;

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
    public MainPage()
    {
        InitializeComponent();
        isRunning = true;
        Clock();
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
    }

    public async void Clock()
    {
        while (isRunning)
        {
            time = time.Add(TimeSpan.FromSeconds(-1));
            Display.Text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00}";
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public void PlayPause(object sender, EventArgs e)
    {
        isRunning = !isRunning;
        PlayPauseButton.Text = isRunning ? "II" : "\u25BA";
        if (isRunning)
        {
            Clock();
            PlayPauseButton.BorderColor = Color.FromArgb("#F1E3F3");
            DisplayBorder.Stroke = Color.FromArgb("#F1E3F3");
        }
        else
        {
            PlayPauseButton.BorderColor = Color.FromArgb("#62BFED");
            DisplayBorder.Stroke = Color.FromArgb("#62BFED");
        }
    }

    public void Stop(object sender, EventArgs e)
    {
        Player.IsVisible = false;
        Main.IsVisible = true;
        isRunning = !isRunning;
    }

    public void AlarmTimer(object sender, EventArgs e) { 
        if (AlarmTimerSwitch.IsToggled == true)
        {
            Timer.Format = "t";
        } 
        else
        {
            Timer.Format = "hh:mm:ss";
        }
    }
}

