namespace SEClockApp;

public partial class Player : ContentPage
{
    //TimeSpan SelectedTime;
    private TimeOnly time = new TimeOnly(00, 10, 00);


    private bool isRunning;

    public Player()
    {
        InitializeComponent();

    }

    public async void PlayPause(object sender, EventArgs e)
    {
        isRunning = !isRunning;

        PlayPauseButton.Text = isRunning ? "Pause" : "Play";

        while (isRunning)
        {
            time = time.Add(TimeSpan.FromSeconds(-1));
            Display.Text = $"{time.Minute:00}:{time.Second:00}";
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    public void Stop(object sender, EventArgs e)
    {

    }

}