using SEClockApp.PopUps;
using CommunityToolkit.Maui.Views;
using SpotifyAPI.Web;
using static System.Formats.Asn1.AsnWriter;

namespace SEClockApp;

public partial class Settings : ContentPage
{
    // Generates a secure random verifier of length 100 and its challenge
    (string, string) verifierAndChallenge = PKCEUtil.GenerateCodes();

    public Settings()
    {
        InitializeComponent();
    }

    private async void ConnectSpotifyHandler(object sender, EventArgs e)
    {
        try
        {
            // Make sure "http://localhost:5000/callback" is in your applications redirect URIs!
            var loginRequest = new LoginRequest(
                new Uri("http://localhost:5000/callback"),
                "be543f38f0f945f7a2384d7575434166", // TODO: shouldn't this be hidden somewhere?
                LoginRequest.ResponseType.Code)
            {
                CodeChallengeMethod = "S256",
                CodeChallenge = verifierAndChallenge.Item2,
                Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
            };
            // Redirects user to allow app to have access to their account
            Uri uri = loginRequest.ToUri();
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString);
        }
    }


    private void AboutOpenHandler(object sender, EventArgs e)
    {
        AboutPopUp aboutPopUp = new();
        this.ShowPopup(aboutPopUp);
    }
}