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

    /// <summary>
    /// Populates the PlaylistVM in MauiProgram with the playlists that we are
    /// getting from the current user's spotify account.
    /// </summary>
    /// <param name="retrievedPlaylists">List of playlists from the user's spotify</param>
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

    /// <summary>
    /// Handles the event of when an item is selected in the Playlist ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void PlaylistsLV_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        // Changes the playlist id in MauiProgram to the newly selected playlist
        Playlist selectedPlaylist = e.SelectedItem as Playlist;
        MauiProgram.playlistId = selectedPlaylist.PlaylistId;
    }
}