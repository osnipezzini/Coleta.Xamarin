using SOColeta.UWP.Data;

using SOColeta.Data;

using SOCore.Utils;

using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DatabasePath))]
namespace SOColeta.UWP.Data
{
    public class DatabasePath : IDBPath
    {
        public DatabasePath()
        { }
        public string GetDbPath()
        {
            string path = SOHelper.AppDataFolder;
            return Path.Combine(path, "AppColeta.db");
        }
    }
}
