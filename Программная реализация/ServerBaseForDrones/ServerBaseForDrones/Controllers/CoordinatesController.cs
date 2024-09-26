using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ServerBaseForDrones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinatesController : ControllerBase
    {
        private static List<Coordinates> _receivedCoordinates = new List<Coordinates>();
        private readonly string missionDirectory = @"C:\Users\pra3t\OneDrive\Документы\QGroundControl\Missions\";

        // Метод для приёма данных с клиента
        [HttpPost]
        public IActionResult PostCoordinates([FromBody] Coordinates coordinates)
        {
            // Сохраняем координаты в список
            _receivedCoordinates.Add(coordinates);

            // Генерация .plan файла и его сохранение
            string planFilePath = GenerateAndSavePlanFile(coordinates);

            // Проверяем, что файл успешно сохранён
            if (string.IsNullOrEmpty(planFilePath))
            {
                return StatusCode(500, new { message = "Error saving the .plan file" });
            }

            // Возвращаем успешный ответ с полученными данными и путём до файла
            return Ok(new { message = "Coordinates received and saved successfully", data = coordinates, planFilePath });
        }

        // Метод для просмотра всех полученных координат
        [HttpGet]
        public IActionResult GetCoordinates()
        {
            return Ok(_receivedCoordinates);
        }

        // Метод для генерации файла .plan и его сохранения в папку миссий QGroundControl
        // Метод для генерации файла .plan и его сохранения в папку миссий QGroundControl
        private string GenerateAndSavePlanFile(Coordinates coordinates)
        {
            try
            {
                // Генерация имени файла на основе временной метки
                string fileName = $"Mission_{DateTime.Now:yyyyMMdd_HHmmss}.plan";
                string filePath = Path.Combine(missionDirectory, fileName);

                // Генерация содержимого .plan файла
                string planContent = GeneratePlanFile(coordinates);

                // Сохранение файла на диск
                System.IO.File.WriteAllText(filePath, planContent);

                return filePath; // Возвращаем путь к файлу
            }
            catch (Exception ex)
            {
                // Логируем или обрабатываем ошибку
                Console.WriteLine($"Error generating/saving .plan file: {ex.Message}");
                return null; // Возвращаем null, если произошла ошибка
            }
        }

        // Метод для генерации содержимого .plan файла на основе координат
        private string GeneratePlanFile(Coordinates coordinates)
        {
            // Пример структуры .plan файла, совместимой с QGroundControl
            var plan = new
            {
                fileType = "Plan",
                groundStation = "QGroundControl", // Необходимый ключ
                geoFence = new
                {
                    circles = new object[] { },
                    polygons = new object[] { },
                    version = 2
                },
                mission = new
                {
                    cruiseSpeed = 5.0,
                    hoverSpeed = 3.0,
                    firmwareType = 3,
                    items = new[]
                    {
                        new
                        {
                            autoContinue = true,
                            command = 22, // Команда Waypoint
                            frame = 3,
                            param1 = 0.0,
                            param2 = 0.0,
                            param3 = 0.0,
                            param4 = 0.0,
                            type = "SimpleItem",
                            coordinate = new[] { coordinates.Latitude, coordinates.Longitude, 50.0 }
                        }
                    },
                    plannedHomePosition = new[] { coordinates.Latitude, coordinates.Longitude, 50.0 },
                    vehicleType = 2,
                    version = 2
                },
                rallyPoints = new
                {
                    points = new object[] { },
                    version = 2
                },
                version = 1
            };

            // Сериализация структуры в JSON формат
            return JsonConvert.SerializeObject(plan, Formatting.Indented);
        }
    }

    // Модель данных координат
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
