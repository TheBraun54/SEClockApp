namespace SEClockApp.PopUps;

public partial class AboutPopUp : ContentPage
{
	public AboutPopUp()
	{
		InitializeComponent();
	}

	private void AboutCloseHandler(object sender, EventArgs e) => Close();
}