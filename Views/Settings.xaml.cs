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
                Scope = new List<string> { Scopes.UserReadEmail }
            };
            BrowserUtil.Open(request.ToUri());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString);
        }
    }

    private static async Task OnImplicitGrantReceived(object sender, ImplictGrantResponse response)
    {
        await _server.Stop();
        var spotify = new SpotifyClient(response.AccessToken);

        // TODO: do calls with Spotify
        var me = await spotify.UserProfile.Current(); // TODO: Delete?

        // TODO: Delete below
        System.Diagnostics.Debug.WriteLine($"Welcome {me.DisplayName}, you're authenticated!");
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
}