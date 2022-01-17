using SOColeta.Data;
using SOColeta.Droid.Data;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AppPath))]
namespace SOColeta.Droid.Data
{
    public class AppPath : IAppPath
    {
        public string Path => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}