using HotelAPI.Context;
using HotelAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(HotelContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var vehicles = await context.Rooms.ToListAsync();
            if (vehicles is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicle = await context.Rooms.SingleOrDefaultAsync(x => x.Id == id);
            if (vehicle is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Room vehicle)
        {
            if (vehicle is null) return BadRequest("hiba");

            context.Rooms.Add(vehicle);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Room vehicle)
        {
            if (vehicle is null) return BadRequest("eh");

            var vehicleToUpdate = await context.Rooms.SingleOrDefaultAsync(x => x.Id == id);
            if (vehicleToUpdate is null) return BadRequest("hiba történt a módosítás során");

            vehicleToUpdate.RoomNumber = vehicle.RoomNumber;
            vehicleToUpdate.Type = vehicle.Type;
            vehicleToUpdate.PricePerNight = vehicle.PricePerNight;
            vehicleToUpdate.IsAvailable = vehicle.IsAvailable;
            await context.SaveChangesAsync();
            return Ok("sikeres módosítás");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleToDelete = await context.Rooms.SingleOrDefaultAsync(x => x.Id == id);
            if (vehicleToDelete is null) return BadRequest("valami hiba történt");

            context.Rooms.Remove(vehicleToDelete);
            await context.SaveChangesAsync();
            return Ok("A törlés sikeres volt");
        }
    }
}
