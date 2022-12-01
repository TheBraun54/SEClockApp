using CommunityToolkit.Maui;

namespace SEClockApp;

public static class MauiProgram
{
    public static PlaylistViewModel playlistVM = new PlaylistViewModel();   // creates PlaylistViewModel objet so we can have a source for the ListView
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // TODO: move this somewhere more secure? where does this go?
        // Sets the environment variable of our app's Client ID which is used in Settings.xaml.cs
        Environment.SetEnvironmentVariable("SPOTIFY_CLIENT_ID", "be543f38f0f945f7a2384d7575434166");

        builder.UseMauiCommunityToolkit();
        return builder.Build();
    }
}
