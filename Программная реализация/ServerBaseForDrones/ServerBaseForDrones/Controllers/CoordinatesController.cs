using Microsoft.AspNetCore.Mvc;
using ServerBaseForDrones;

[Route("api/[controller]")]
[ApiController]
public class CoordinatesController : ControllerBase
{
    private static List<Coordinates> _receivedCoordinates = new List<Coordinates>();

    // Метод для приёма данных с клиента
    [HttpPost]
    public IActionResult PostCoordinates([FromBody] Coordinates coordinates)
    {
        // Сохраняем координаты в список
        _receivedCoordinates.Add(coordinates);

        // Возвращаем успешный ответ с полученными данными
        return Ok(new { message = "Coordinates received successfully", data = coordinates });
    }

    // Метод для просмотра всех полученных координат
    [HttpGet]
    public IActionResult GetCoordinates()
    {
        return Ok(_receivedCoordinates);
    }
}
