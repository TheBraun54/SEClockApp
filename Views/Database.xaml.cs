using Npgsql;
using System.Collections.ObjectModel;

namespace SEClockApp;

public partial class Database : ContentPage
{
    String connectionString;
    ObservableCollection<Playlist> data = new ObservableCollection<Playlist>();

    /// <summary>
    /// Creates connection string to be used to connect to bit.io db
    /// </summary>
    public Database()
    {
        InitializeComponent();
        connectionString = InitializeConnectionString();
        SetItemSource();
    }

    /// <summary>
    /// Creates the connection string to be utilized throughout the program
    /// </summary>
    public String InitializeConnectionString()
    {
        var bitHost = "db.bit.io";
        var bitApiKey = "v2_3vTUY_RVB7sHAEteABJNccQ6eULDz";

        var bitUser = "paulhwangj";
        var bitDbName = "paulhwangj/SEClockProject";

        return connectionString = $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";
    }

    /// <summary>
    ///  Sets the item source of the ListView to have data appear 
    /// </summary>
    public void SetItemSource()
    {
        string sql = "SELECT * FROM playlists";

        // clear entries, ordering of entries is going to change
        data.Clear();

        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var cmd = new NpgsqlCommand(sql, con);
        using NpgsqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            // populates accordingly
            data.Add(new Playlist(reader[0] as String, (int)reader[1]));
        }
        con.Close();

        DummyLV.ItemsSource = data;
    }
}