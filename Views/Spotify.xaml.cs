using SpotifyAPI.Web;
using System;
using System.ComponentModel;

namespace SEClockApp;
public partial class Spotify : ContentPage
{
    public Spotify()
    {
        InitializeComponent();
        Main();
    }

    async Task Main()
    {
        var spotify = new SpotifyClient("BQBm5T_4TP6txgHbWoQNe-3IHcZpE38YxEmYXjHsDdzTtgztNy-4zpbCCzWpcA5XTEeaRLatYlq2S1sOe6Iv5P6J1gevl7ion7tVNmxbp7t5MPkNbqnOmcmrOVlcuPqH8LqBb6BZsrni28D9kldKna84rdN4U3hze9YxuMVU9RIs29IjYk3yas5PmVvqPBzahJk");

        var track = await spotify.Tracks.Get("6OckTB2amWc3Jfa47Zg01U");
        Title.Text = track.Name;
    }
}