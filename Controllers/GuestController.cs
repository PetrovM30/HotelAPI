using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI.Context;
using HotelAPI.Entities;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController(HotelContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await context.Guests.ToListAsync();
            if (customers is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await context.Guests.SingleOrDefaultAsync(x => x.Id == id);
            if (customer is null) return BadRequest("nem sikerült a lekérdezés");

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Guest customer)
        {
            if (customer is null) return BadRequest("hiba");

            context.Guests.Add(customer);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Guest customer)
        {
            if (customer is null) return BadRequest("eh");

            var customerToUpdate = await context.Guests.SingleOrDefaultAsync(x => x.Id == id);
            if (customerToUpdate is null) return BadRequest("hiba történt a módosítás során");

            customerToUpdate.Name = customer.Name;
            customerToUpdate.Email = customer.Email;
            customerToUpdate.PhoneNumber = customer.PhoneNumber;
            await context.SaveChangesAsync();
            return Ok("sikeres módosítás");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customerToDelete = await context.Guests.SingleOrDefaultAsync(x => x.Id == id);
            if (customerToDelete is null) return BadRequest("valami hiba történt");

            context.Guests.Remove(customerToDelete);
            await context.SaveChangesAsync();
            return Ok("A törlés sikeres volt");
        }
    }
}
