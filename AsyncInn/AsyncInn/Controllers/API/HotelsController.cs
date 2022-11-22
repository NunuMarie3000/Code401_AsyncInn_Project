using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;

namespace AsyncInn.Controllers.API
{
  [Route("api/[controller]")]
  [ApiController]
  public class HotelsController: ControllerBase
  {
    private readonly AsyncInnDbContext _context;

    public HotelsController( AsyncInnDbContext context )
    {
      _context = context;
    }

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
    {
      return await _context.Hotels.ToListAsync();
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ModelsDTO.HotelDTO>> GetHotel( int id )
    {
      var hotel = await _context.Hotels.FindAsync(id);

      // querying all hotelroom objects associated with the given hotel
      var hotelrooms = from item in _context.HotelRoom
                             where item.HotelID == id
                             select item;
      

      if (hotel == null)
      {
        return NotFound();
      }

      // new hoteldto based on input id
      ModelsDTO.HotelDTO hoteldto = new ModelsDTO.HotelDTO() { City = hotel.City, Country = hotel.Country, Name = hotel.Name, Phone = hotel.Phone, State = hotel.State, StreetAddress = hotel.StreetAddress };
      foreach (var item in hotelrooms)
      {
        // creates new instance of HotelRoomDTO
        ModelsDTO.HotelRoomDTO newhotelroomdto = new ModelsDTO.HotelRoomDTO() { Hotel = item.Hotel, PetFriendly = item.PetFriendly, Rate = item.Rate, RoomNumber = item.RoomNumber, Room = item.Room };
        // adds new hotelroomdto to the list inside of hoteldto object
        hoteldto.Rooms.Add(newhotelroomdto);

        // while i'm here, each hotelroom object has a reference to the room object, which contains a list of roomAmenities objects, i can create dtos for them while i'm here
        foreach(var i in item.Room.RoomAmenities)
        {

        }
      }


      return hoteldto;
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel( int id, Hotel hotel )
    {
      if (id != hotel.Id)
      {
        return BadRequest();
      }

      _context.Entry(hotel).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!HotelExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel( Hotel hotel )
    {
      _context.Hotels.Add(hotel);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel( int id )
    {
      var hotel = await _context.Hotels.FindAsync(id);
      if (hotel == null)
      {
        return NotFound();
      }

      _context.Hotels.Remove(hotel);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool HotelExists( int id )
    {
      return _context.Hotels.Any(e => e.Id == id);
    }
  }
}
