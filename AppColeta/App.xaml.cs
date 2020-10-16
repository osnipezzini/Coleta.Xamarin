using AppColeta.Models;
using AppColeta.Services;
using SOTechLib.Utils;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace AppColeta
{
    public partial class App : Application
    {
        public static Inventario Inventario { get; set; }
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            
        }
        public async static void CheckValid()
        {
            if (Helper.TKT)
            {
                var buildDate = new DateTime(2020, 10, 16);
                var today = DateTime.Today;
                var valid = today - buildDate;
                if (valid.Days > 3)
                {
                    await Current.MainPage.DisplayAlert("Expirado", "Esse aplicativo expirou, contate o suporte para obter uma nova versão!", "OK");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }
        public static DateTime GetLinkerTimestampUtc(Assembly assembly)
        {
            var location = assembly.Location;
            return GetLinkerTimestampUtc(location);
        }

        public static DateTime GetLinkerTimestampUtc(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            var headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(secondsSince1970);
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
