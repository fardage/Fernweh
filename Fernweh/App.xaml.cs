﻿using Fernweh.Views;
using MonkeyCache.FileStore;
using Xamarin.Forms;

namespace Fernweh
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = "ch.tseng.Fernweh";
            Barrel.Current.EmptyExpired();

            MainPage = new NavigationPage(new TripsPage());
        }

        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}