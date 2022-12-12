// Primary Author: Paul Hwang
// Secondary Author: Brady Braun
// Reviewer: Brady Braun 

using SpotifyAPI.Web;
using System;
using System.ComponentModel;

namespace SEClockApp;
public partial class Spotify : ContentPage
{
    public Spotify()
    {
        InitializeComponent();
        PlaylistsLV.ItemsSource = MauiProgram.playlistVM.GetPlaylists();     // Sets the ItemSource of the ListView to the playlistsVM in MauiProgram.cs
    }

    public static void PopulatePlaylistGrid(IList<SimplePlaylist> retrievedPlaylists)
    {
        foreach (SimplePlaylist playlist in retrievedPlaylists)
        {
            // TODO: Delete, just checking it even acquires the playlists
            System.Diagnostics.Debug.WriteLine($"{playlist.Name}");
            System.Diagnostics.Debug.WriteLine($"{playlist.Images[0].Url}");
            System.Diagnostics.Debug.WriteLine($"{playlist.Id}");

            // Populates the ListView in Spotify.xaml to show the playlists
            MauiProgram.playlistVM.AddPlaylist(new Playlist(playlist.Name, 0, playlist.Images[0].Url, playlist.Id));
        }
    }
}