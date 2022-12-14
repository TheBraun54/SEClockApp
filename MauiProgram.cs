using CommunityToolkit.Maui;
using SpotifyAPI.Web;

namespace SEClockApp;

// Primary Author: Paul Hwang
// Secondary Author: Brady Braun
// Reviewer: Brady Braun

public static class MauiProgram
{
    public static SpotifyPlaylistViewModel spotifyPlaylistVM = new SpotifyPlaylistViewModel();   // creates PlaylistViewModel objet so we can have a source for the ListView
    public static bool isSpotify = true;
    public static SpotifyPlaylist selectedPlaylist;
    public static SpotifyClient spotify = null;            // spotify client to be used throughout the application

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
