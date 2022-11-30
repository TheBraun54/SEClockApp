using SEClockApp.PopUps;
using CommunityToolkit.Maui.Views;

namespace SEClockApp;

public partial class Settings : ContentPage
{

    public Settings()
    {
        InitializeComponent();
    }

    private void ConnectSpotifyHandler(object sender, EventArgs e)
    {

    }
     

    private void AboutOpenHandler(object sender, EventArgs e)
    {
        AboutPopUp aboutPopUp = new();
        this.ShowPopup(aboutPopUp);
    }
}