using AppColeta.Data;
using AppColeta.Droid.Data;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DatabasePath))]
namespace AppColeta.Droid.Data
{
    public class DatabasePath : IDBPath
    {
        public DatabasePath()
        { }
        public string GetDbPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "AppColeta.db");
        }
    }
}