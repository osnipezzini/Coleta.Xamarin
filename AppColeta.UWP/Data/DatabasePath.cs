using AppColeta.Data;
using AppColeta.UWP.Data;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DatabasePath))]
namespace AppColeta.UWP.Data
{
    public class DatabasePath : IDBPath
    {
        public DatabasePath()
        { }
        public string GetDbPath()
        {
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "SOTech");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return Path.Combine(path, "AppColeta.db");
        }
    }
}
