
namespace SEClockApp;
/*
 * Primary Author: Brady Braun
 * Secondary Author: Zach La Vake
 * Reviewer: 
 */

public partial class MainPage : ContentPage
{
    private int hours;
    private int minutes;
    private int seconds;
    private TimeOnly time;

    private bool isRunning;  
    public MainPage()
    {
        InitializeComponent();
    }

    private void StartClock(object sender, EventArgs e)
    {
        Main.IsVisible = false;
        Player.IsVisible = true;
        isRunning = true;
        time = new TimeOnly(hours, minutes, seconds);
        TimerClock();
    }

    public async void TimerClock()
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
            TimerClock();
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
        isRunning = false;
        TimerClock();
        Reset();
    }

    public void Reset()
    {
        Hours.Text = "00";
        Minutes.Text = "00";
        Seconds.Text = "00";
        time = new TimeOnly(0, 0, 0);
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
}

