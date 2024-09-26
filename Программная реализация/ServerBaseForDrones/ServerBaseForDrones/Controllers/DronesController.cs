using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServerBaseForDrones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DronesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DronesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для добавления дрона
        [HttpPost]
        public async Task<IActionResult> AddDrone([FromBody] Drone drone)
        {
            _context.Drones.Add(drone);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Drone added successfully", drone });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDroneStatus(int id, [FromBody] UpdateDroneStatusDto dto)
        {
            var drone = await _context.Drones.FindAsync(id);

            if (drone == null)
            {
                return NotFound(new { message = "Drone not found" });
            }

            drone.Status = dto.Status;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Drone status updated successfully", drone });
        }

        // Метод для получения всех дронов
        [HttpGet]
        public async Task<IActionResult> GetDrones()
        {
            var drones = await _context.Drones.ToListAsync();
            return Ok(drones);
        }
    }
}
