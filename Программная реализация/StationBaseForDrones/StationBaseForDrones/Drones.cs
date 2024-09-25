using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StationBaseForDrones
{
    [Activity(Label = "DronesActivity")]
    public class DronesActivity : Activity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_drones);

            var dronesListView = FindViewById<ListView>(Resource.Id.dronesListView);

            // Получаем информацию о дронах с сервера
            var drones = await GetDronesFromServer();

            Android.Util.Log.Debug("DronesActivity", $"Количество дронов: {drones.Count}");

            // Устанавливаем адаптер для отображения списка дронов
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, drones);
            dronesListView.Adapter = adapter;
        }

        private async Task<List<string>> GetDronesFromServer()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://192.168.1.38:5000/api/Drones"; // Адрес API для получения списка дронов
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var drones = JsonConvert.DeserializeObject<List<Drone>>(json);
                        var droneInfoList = new List<string>();

                        foreach (var drone in drones)
                        {
                            droneInfoList.Add($"ID: {drone.Id}, Status: {drone.Status}");
                        }

                        return droneInfoList;
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Ошибка при получении данных: " + ex.Message, ToastLength.Short).Show();
                }
            }

            return new List<string> { "Нет данных" };
        }

        public class Drone
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }
    }
}