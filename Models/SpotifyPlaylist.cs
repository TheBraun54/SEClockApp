using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SEClockApp;

public class SpotifyPlaylist : ObservableObject
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string PlaylistId { get; set; }
    public TimeSpan Duration { get; set; }
    public List<SpotifyTrack> Songs { get; set; }

    /// <summary>
    /// Constructor for a SpotifyPlaylist
    /// </summary>
    /// <param name="name">the name of the playlist</param>
    /// <param name="imageUrl">the url to the playlist cover</param>
    /// <param name="playlistId">the id of the playlist</param>
    public SpotifyPlaylist(string name, string imageUrl, string playlistId, TimeSpan duration, List<SpotifyTrack> songs)
    {
        this.Name = name;
        this.ImageUrl = imageUrl;
        this.PlaylistId = playlistId;
        this.Duration = duration;
        this.Songs = songs;
    }

    /// <summary>
    /// Represents a spotify track
    /// </summary>
    public class SpotifyTrack
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public TimeSpan Duration;
        public string Uri { get; set; }

        public SpotifyTrack(string name, string id, TimeSpan duration, string uri)
        {
            this.Name = name;
            this.Id = id;
            this.Duration = duration;
            this.Uri = uri;
        }
    }
}

