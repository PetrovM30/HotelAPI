using HotelAPI.Context;
using HotelAPI.DTO;
using HotelAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(HotelContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bookings = await context.Bookings.ToListAsync();
            if (bookings is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await context.Bookings.SingleOrDefaultAsync(x => x.Id == id);
            if (booking is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookingDTO bookingDTO)
        {
            var booking = new Booking
            {
                GuestId = bookingDTO.GuestId,
                RoomId = bookingDTO.RoomId,
                CheckInDate = bookingDTO.CheckInDate,
                CheckOutDate = bookingDTO.CheckOutDate,
            };
            if (booking is null) return BadRequest("hiba");

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
        {
            if (booking is null) return BadRequest("eh");

            var bookingToUpdate = await context.Bookings.SingleOrDefaultAsync(x => x.Id == id);
            if (bookingToUpdate is null) return BadRequest("hiba történt a módosítás során");

            bookingToUpdate.GuestId = booking.GuestId;
            bookingToUpdate.RoomId = booking.RoomId;
            bookingToUpdate.CheckInDate = booking.CheckInDate;
            bookingToUpdate.CheckOutDate = booking.CheckOutDate;
            await context.SaveChangesAsync();
            return Ok("sikeres módosítás");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookingToDelete = await context.Bookings.SingleOrDefaultAsync(x => x.Id == id);
            if (bookingToDelete is null) return BadRequest("valami hiba történt");

            context.Bookings.Remove(bookingToDelete);
            await context.SaveChangesAsync();
            return Ok("A törlés sikeres volt");
        }
    }
}
