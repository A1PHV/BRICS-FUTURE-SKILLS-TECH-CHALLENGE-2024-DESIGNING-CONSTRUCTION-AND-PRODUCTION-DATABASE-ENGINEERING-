using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoordinateSender
{
    public partial class App : Application
    {
        MapPage _mapPage;
        RoutesPage _routesPage;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            _mapPage = new MapPage();
            _routesPage = new RoutesPage();
        }

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
