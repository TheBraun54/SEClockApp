namespace SEClockApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Below are added to register the routes to each specific page
        Routing.RegisterRoute(nameof(Settings), typeof(Settings));
        Routing.RegisterRoute(nameof(Player), typeof(Player));
        Routing.RegisterRoute(nameof(Spotify), typeof(Spotify));
        Routing.RegisterRoute(nameof(Youtube), typeof(Youtube));
        Routing.RegisterRoute(nameof(Local), typeof(Local));
    }
}
