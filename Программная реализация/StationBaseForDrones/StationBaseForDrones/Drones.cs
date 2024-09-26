using Android.App;
using Android.OS;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace StationBaseForDrones
{
    [Activity(Label = "DronesActivity")]
    public class DronesActivity : Activity
    {
        private List<Drone> _drones;
        private Drone _selectedDrone;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_drones);

            var dronesListView = FindViewById<ListView>(Resource.Id.dronesListView);
            var statusInput = FindViewById<EditText>(Resource.Id.statusInput);
            var updateStatusButton = FindViewById<Button>(Resource.Id.updateStatusButton);

            // Получаем информацию о дронах с сервера
            _drones = await GetDronesFromServer();

            if (_drones.Count == 0)
            {
                Toast.MakeText(this, "Нет данных о дронах", ToastLength.Short).Show();
                return;
            }

            // Устанавливаем адаптер для отображения списка дронов
            var droneNames = new List<string>();
            foreach (var drone in _drones)
            {
                droneNames.Add($"ID: {drone.Id}, Status: {drone.Status}");
            }

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, droneNames);
            dronesListView.Adapter = adapter;

            // Обрабатываем выбор дрона из списка
            dronesListView.ItemClick += (sender, e) =>
            {
                _selectedDrone = _drones[e.Position];
                Toast.MakeText(this, $"Выбран дрон ID: {_selectedDrone.Id}", ToastLength.Short).Show();
            };

            // Обрабатываем обновление статуса
            updateStatusButton.Click += async (sender, e) =>
            {
                if (_selectedDrone == null)
                {
                    Toast.MakeText(this, "Пожалуйста, выберите дрона", ToastLength.Short).Show();
                    return;
                }

                var newStatus = statusInput.Text;
                if (string.IsNullOrEmpty(newStatus))
                {
                    Toast.MakeText(this, "Введите новый статус", ToastLength.Short).Show();
                    return;
                }

                // Обновляем статус дрона на сервере
                await UpdateDroneStatus(_selectedDrone.Id, newStatus);
                Toast.MakeText(this, "Статус обновлен", ToastLength.Short).Show();

                // Обновляем список дронов
                _drones = await GetDronesFromServer();
                droneNames.Clear();
                foreach (var drone in _drones)
                {
                    droneNames.Add($"ID: {drone.Id}, Status: {drone.Status}");
                }
                adapter.NotifyDataSetChanged();
            };
        }

        private async Task<List<Drone>> GetDronesFromServer()
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
                        return drones;
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Ошибка при получении данных: " + ex.Message, ToastLength.Short).Show();
                }
            }

            return new List<Drone>();
        }

        private async Task UpdateDroneStatus(int droneId, string newStatus)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var updatedDrone = new { Status = newStatus }; // Объект с полем "Status"
                    var json = JsonConvert.SerializeObject(updatedDrone);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    string url = $"http://192.168.1.38:5000/api/Drones/{droneId}";
                    var response = await client.PutAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Ошибка обновления статуса");
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Ошибка обновления статуса: " + ex.Message, ToastLength.Short).Show();
                }
            }
        }

        public class Drone
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }
    }
}