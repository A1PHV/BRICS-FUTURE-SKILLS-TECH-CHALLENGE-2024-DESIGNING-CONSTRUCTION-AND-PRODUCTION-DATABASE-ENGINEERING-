using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoordinateSender
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Переход на RoutesPage
        private async void OnGoToRoutesPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.RoutesPage());
        }

        // Переход на MapPage
        private async void OnGoToMapPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage());
        }
    }
}
