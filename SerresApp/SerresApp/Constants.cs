using SerresApp.Models;

using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SerresApp
{
    public class Constants
    {
        public static readonly string GoogleApiKey = "AIzaSyAbe9MnGMU3M9DP3BHmSYqkKpW8qRSNePU";

        public static readonly string APIConnectionBaseAddress = "https://esa.exeo.site/";


        // {AssemblyName}.Json.{FileName}.json
        public static readonly string LightThemeMapPath = "SerresApp.Json.travelstyle.json";
        public static readonly string DarkThemeMapPath = "SerresApp.Json.darkstyle.json";

        public static readonly string GoogleMapsBaseAddress = "https://maps.googleapis.com";
        public static readonly string WeatherApiBaseAddress = "https://api.openweathermap.org";

        public static readonly string JsonFolder = "Json";
        public static readonly string JsonTitle = "serres";

        public const string DB_Name = "esapoisdb3.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static readonly ObservableCollection<OnboardingModel> Items = new ObservableCollection<OnboardingModel>
            {
                new OnboardingModel
                {
                    Title = "Welcome to Tourist Guide",
                    Content = "Tourist Guide helps you find out about the best places in Place.",
                    ImageUrl = "welcome.png"
                },
                new OnboardingModel
                {
                    Title = "Discover the best places",
                    Content = "If you would like to find the best places around you, please allow Tourist Guide to access your location while using the app.",
                    ImageUrl = "maps.png"
                },
                new OnboardingModel
                {
                    Title = "Places" ,
                    Content = "Find out everything about the best places." ,
                    ImageUrl = "data.png"
                }
            };

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath , DB_Name);
            }
        }

    }
}
