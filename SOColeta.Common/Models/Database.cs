using SOCore.Models;

using System.Text;

namespace SOColeta.Common.Models;

public class Database : ObservableObject
{
    private string _host = "localhost";
    public string Host { get => _host; set => SetProperty(ref _host, value); }
    private string _port = "5432";
    public string Port { get => _port; set => SetProperty(ref _port, value); }
    private string _name;
    public string Name { get => _name; set => SetProperty(ref _name, value); }
    private string _user = "postgres";
    public string Username { get => _user; set => SetProperty(ref _user, value); }
    private string _pass;
    public string Password { get => _pass; set => SetProperty(ref _pass, value); }
    private string _type;
    public string Type { get => _type; set => SetProperty(ref _type, value); }
    public bool IsValid { get => !string.IsNullOrEmpty(ToString()); }
    public string ConnectionString => ToString();

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Host) || string.IsNullOrEmpty(Name))
            return string.Empty;

        var databaseStr = new StringBuilder();
        databaseStr.Append($"Host={Host};");

        if (!string.IsNullOrEmpty(Port))
            databaseStr.Append($"Port={Port};");

        databaseStr.Append($"Database={Name};");

        if (!string.IsNullOrEmpty(Username))
            databaseStr.Append($"Username={Username};");
        else
            databaseStr.Append("Username=postgres");

        if (!string.IsNullOrEmpty(Password))
            databaseStr.Append($"Password={Password};");

        databaseStr.Append("Client Encoding=LATIN1;");
        databaseStr.Append("Encoding=LATIN1;");
        return databaseStr.ToString();
    }

    public Database Clone()
    {
        return (Database)MemberwiseClone();
    }
}
