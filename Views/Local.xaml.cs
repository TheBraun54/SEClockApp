using static SEClockApp.Logic.Logic;

namespace SEClockApp;

// Primary Author: Zach La Vake
// Secondary Author: Qadar Isse
// Reviewer: Brady Braun

public partial class Local : ContentPage
{
    //List<string> SelectedDirectories = new List<string>();

    PickOptions options = new()
    {
        PickerTitle = "Select a file in the music directory",
    };

    public Local()
    {
        InitializeComponent();
    }

    public async void addHandler(object sender, EventArgs e)
    {
        string Dir = await PickDirectory(options);
        if (Dir == null) { return; }
        Button btn = (Button)sender;
        /*if (btn.Text != "+")
        {
            Directories.RemoveDirectory(Dir);
        }*/

        Directories.AddDirectory(Dir); 
        btn.Text = Dir;
        btn.FontSize = 12;
    }

    public async Task<string> PickDirectory(PickOptions options)
    {
        string Dir = null;
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                var DirLength = result.FullPath.Length - result.FileName.Length;
                Dir = result.FullPath.Substring(0, DirLength);
            }
        }
        catch (Exception ex)
        {
            // user canceled or something
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return Dir;
    }
}