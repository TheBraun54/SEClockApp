// Primary Author: Paul Hwang
// Secondary Author: Brady Braun
// Reviewer: Brady Braun 

using SpotifyAPI.Web;
using System;
using System.ComponentModel;
using static SEClockApp.SpotifyPlaylist;

namespace SEClockApp;
public partial class Spotify : ContentPage
{
    public Spotify()
    {
        InitializeComponent();
        PlaylistsLV.ItemsSource = MauiProgram.spotifyPlaylistVM.GetPlaylists();     // Sets the ItemSource of the ListView to the playlistsVM in MauiProgram.cs
    }

    /// <summary>
    /// Populates the PlaylistVM in MauiProgram with the playlists that we are
    /// getting from the current user's spotify account.
    /// </summary>
    /// <param name="retrievedPlaylists">List of playlists from the user's spotify</param>
    public async static void PopulatePlaylistGrid(IList<SimplePlaylist> retrievedPlaylists)
    {
        foreach (SimplePlaylist playlist in retrievedPlaylists)
        {
            List<SpotifyTrack> songs = new List<SpotifyTrack>();

            // Gets all the songs from the playlists and adds them to a list
            var playlistGetItemsRequest = new PlaylistGetItemsRequest();
            playlistGetItemsRequest.Fields.Add("items(track(id,name,type,duration_ms,uri))"); // 'type' is required
            var playlistItems = await MauiProgram.spotify.PaginateAll(await MauiProgram.spotify.Playlists.GetItems($"{playlist.Id}", playlistGetItemsRequest));
            foreach (PlaylistTrack<IPlayableItem> item in playlistItems)
            {
                // Ensure that the current track is a song
                if (item.Track is FullTrack track)
                {
                    songs.Add(new SpotifyTrack(track.Name, track.Id, TimeSpan.FromMilliseconds(track.DurationMs), track.Uri));


                //    // TODO: delete
                //    System.Diagnostics.Debug.WriteLine($"{track.Name} -- {track.Uri}");
                }
            }

            // Creates a new SpotifyPlaylist and adds it to the ListView
            MauiProgram.spotifyPlaylistVM.AddPlaylist(new SpotifyPlaylist(playlist.Name, playlist.Images[0].Url, playlist.Id, songs));
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
        SpotifyPlaylist selectedPlaylist = e.SelectedItem as SpotifyPlaylist;
        MauiProgram.selectedPlaylist = selectedPlaylist;
    }
}