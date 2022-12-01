using CommunityToolkit.Maui.Views;

namespace SEClockApp.PopUps;
/*
 * Primary Author: Brady Braun
 * Secondary Author: None
 * Reviewer: 
 */
public partial class AboutPopUp : Popup
{
	public AboutPopUp()
	{
		InitializeComponent();
	}

	private void AboutCloseHandler(object sender, EventArgs e) => Close();
}