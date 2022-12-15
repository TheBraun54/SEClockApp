// Primary Author: Paul Hwang
// Secondary Author: Brady Braun
// Reviewer: Brady Braun 

using SEClockApp.PopUps;
using CommunityToolkit.Maui.Views;
using SpotifyAPI.Web;
using static System.Formats.Asn1.AsnWriter;
using SpotifyAPI.Web.Auth;

namespace SEClockApp;

public partial class Settings : ContentPage
{
    private static readonly string? clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
    // Make sure "http://localhost:5000/callback" is in your spotify application as redirect uri!
    private static readonly EmbedIOAuthServer _server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);

    public Settings()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Starts the server and processes the request to the whitelisted redirect uri
    /// then when the application gets auth rights, OnImplicitGrantRecieved does the spotify calls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ConnectSpotifyHandler(object sender, EventArgs e)
    {
        try
        {
            await _server.Start();

            _server.ImplictGrantReceived += OnImplicitGrantReceived;
            _server.ErrorReceived += OnErrorReceived;

            var request = new LoginRequest(_server.BaseUri, clientId, LoginRequest.ResponseType.Token)
            {
                Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserModifyPlaybackState,
                                           Scopes.UserReadCurrentlyPlaying, Scopes.UserReadPlaybackState,
                                           Scopes.PlaylistReadPrivate }
            };

            // Opens the browser to have user sign in
            await Browser.Default.OpenAsync(request.ToUri(), BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString);
        }
    }

    private async Task OnImplicitGrantReceived(object sender, ImplictGrantResponse response)
    {
        await _server.Stop();
        MauiProgram.spotify = new SpotifyClient(response.AccessToken);

        var me = await MauiProgram.spotify.UserProfile.Current();
        var playlists = await MauiProgram.spotify.PaginateAll(await MauiProgram.spotify.Playlists.CurrentUsers().ConfigureAwait(false));

        // Prints all public playlists for the signed in user
        foreach (SimplePlaylist playlist in playlists)
        {
            System.Diagnostics.Debug.WriteLine($"{playlist.Name}");
            System.Diagnostics.Debug.WriteLine($"{playlist.Id}");
        }
        System.Diagnostics.Debug.WriteLine($"Welcome {me.DisplayName}, you're authenticated!");

        // Populate the Spotify.xaml with the user's playlists
        Spotify.PopulatePlaylistGrid(playlists);
    }

    private static async Task OnErrorReceived(object sender, string error, string state)
    {
        Console.WriteLine($"Aborting authorization, error received: {error}");
        await _server.Stop();
    }


    private void AboutOpenHandler(object sender, EventArgs e)
    {
        AboutPopUp aboutPopUp = new();
        this.ShowPopup(aboutPopUp);
    }

    /// <summary>
    /// Determines whether the user wants to play music from the local directory
    /// or from their selected spotify playlist
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void SpotifyLocal(object sender, EventArgs e)
    {
        // If the switch is on local, we aren't playing spotify so set it to false
        // else we are wanting to play spotify so set it to true
        MauiProgram.isSpotify = SpotifyOrLocalSwitch.IsToggled ? false : true;

        // Changes the toggle's color depending on what music source is selected
        if (MauiProgram.isSpotify)
        {
            SpotifyOrLocalSwitch.ThumbColor = Color.FromHex("1DB954");
        }
        else
        {
            SpotifyOrLocalSwitch.ThumbColor = Colors.Yellow;
        }
    }
}