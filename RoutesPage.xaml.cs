using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CoordinateSender
{
    public partial class RoutesPage : ContentPage
    {
        public RoutesPage()
        {
            //InitializeComponent();
            LoadRoutesFromServer();
        }

        private async void LoadRoutesFromServer()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Замените на ваш URL
                    string serverUrl = "http://yourserver.com/api/routes";
                    HttpResponseMessage response = await client.GetAsync(serverUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        var routes = JsonConvert.DeserializeObject<List<RouteModel>>(jsonData);

                        //routesListView.ItemsSource = routes;
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to load routes.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }

    // Модель маршрута для десериализации данных с сервера
    public class RouteModel
    {
        public string RouteName { get; set; }
        public string Coordinates { get; set; }
    }
}