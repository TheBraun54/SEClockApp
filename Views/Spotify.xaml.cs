using SpotifyAPI.Web;
using System;
using System.ComponentModel;

namespace SEClockApp;
public partial class Spotify : ContentPage
{
    List<Playlist> playlists = new List<Playlist>();
    public Spotify()
    {
        InitializeComponent();
    }

    public static void PopulatePlaylistGrid(IList<SimplePlaylist> retrievedPlaylists)
    {
        
    }
}