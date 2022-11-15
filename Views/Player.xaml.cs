namespace SEClockApp;

public partial class Player : ContentPage
{
    //TimeSpan SelectedTime;
    private TimeOnly time = new TimeOnly(01, 30, 00);

    private bool isRunning;

    public Player()
    {
        InitializeComponent();
        isRunning = true;
        Clock();
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

    public async void Stop(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}