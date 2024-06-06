using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace EcoTrack
{
    public partial class App : Application
    {
        static Database database;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EcoTrack.db3"));
                }
                return database;
            }
        }
    }
}
