using AppColeta.Data;
using AppColeta.Droid.Data;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AppPath))]
namespace AppColeta.Droid.Data
{
    public class AppPath : IAppPath
    {
        public string Path => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}