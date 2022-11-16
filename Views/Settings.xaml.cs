using SEClockApp.PopUps;

namespace SEClockApp;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
    }

    private void AboutOpenHandler(object sender, EventArgs e)
    {
        AboutPopUp aboutPopUp = new AboutPopUp();
        this.ShowPopUp(aboutPopUp);

    }
}