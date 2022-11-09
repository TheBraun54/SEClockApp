namespace SEClockApp;

public partial class MainPage : ContentPage
{

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

    private async void StartClock(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"Player");
    }
}

