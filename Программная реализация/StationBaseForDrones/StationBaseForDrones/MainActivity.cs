using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Android.Content;

namespace StationBaseForDrones
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public async void SendCoordinates(double latitude, double longitude)
        {
            var coordinates = new { Latitude = latitude, Longitude = longitude };
            var json = JsonConvert.SerializeObject(coordinates);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://192.168.1.38:5000/api/coordinates", content);
                if (response.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, "Координаты отправлены", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Ошибка отправки", ToastLength.Short).Show();
                }
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var latitudeInput = FindViewById<EditText>(Resource.Id.latitudeInput);
            var longitudeInput = FindViewById<EditText>(Resource.Id.longitudeInput);
            var sendButton = FindViewById<Button>(Resource.Id.sendButton);
            var showDronesButton = FindViewById<Button>(Resource.Id.showDronesButton);

            sendButton.Click += (sender, e) =>
            {
                if (double.TryParse(latitudeInput.Text, out double latitude) &&
                    double.TryParse(longitudeInput.Text, out double longitude))
                {
                    SendCoordinates(latitude, longitude);
                }
                else
                {
                    Toast.MakeText(this, "Неверные координаты", ToastLength.Short).Show();
                }
            };

            // Обработчик нажатия на кнопку для перехода на страницу дронов
            showDronesButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(DronesActivity));
                StartActivity(intent);
            };
        }



    }
}
