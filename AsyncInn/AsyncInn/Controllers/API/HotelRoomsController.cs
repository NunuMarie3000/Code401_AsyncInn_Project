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
  //[Route("api/[controller]")]
  [Route("api/Hotels/{hotelId}/Rooms")]
  [ApiController]
  public class HotelRoomsController: ControllerBase
  {
    private readonly AsyncInnDbContext _context;

    public HotelRoomsController( AsyncInnDbContext context )
    {
      _context = context;
    }

    // GET: api/Hotels/{hotelId}/Rooms
    // get all hotel rooms with associated hotelId
    [HttpGet]
    //[Route("api/Hotels/{hotelId}/Rooms")]
    public async Task<ActionResult<IEnumerable<HotelRoom>>> GetHotelRoom( int hotelId )
    {
      return await _context.HotelRoom.Where(hotelroom => hotelroom.Hotel.Id == hotelId).ToListAsync();
    }

    // GET: api/HotelRooms/5
    //[HttpGet("{id}")]
    [HttpGet("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
    public async Task<ActionResult<HotelRoom>> GetHotelRoom( int hotelId, int roomNumber )
    {
      var hotelRoom = await _context.HotelRoom.Where(hotelroom => hotelroom.Hotel.Id == hotelId && hotelroom.RoomNumber == roomNumber).FirstOrDefaultAsync();

      if (hotelRoom == null)
      {
        return NotFound();
      }
      return hotelRoom;
    }

    // PUT: api/HotelRooms/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
    public async Task<IActionResult> PutHotelRoom( int hotelId, int roomNumber, HotelRoom updatedHotelRoom )
    {
      //var chosen = _context.HotelRoom.Where(hotelroom => hotelroom.HotelID == hotelId && hotelroom.RoomID == roomNumber).FirstOrDefault();
      if (hotelId != updatedHotelRoom.Id)
      {
        return BadRequest();
      }

      _context.Entry(updatedHotelRoom).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!HotelRoomExists(hotelId))
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

    // POST: api/HotelRooms
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //[HttpPost]
    [HttpPost("/api/Hotels/{hotelId}/Rooms")]
    public async Task<ActionResult<HotelRoom>> PostHotelRoom( int hotelId, [Bind("Id, HotelId, RoomId, RoomNumber, Rate, PetFriendly")] HotelRoom hotelroom)
    {
      // add a room to a hotel
      var hotel = _context.Hotels.Where(h => h.Id == hotelId).FirstOrDefault();
      
      // we need to add the hotelRoom to this, but where are we getting the hotelRoom object from?
      // i guess from the body? do we have to do that bind thing i keep seeing?
      //_context.HotelRoom.Add(hotelRoom);
      if(ModelState.IsValid)
      {
        hotel.HotelRoom.Add(hotelroom);
        await _context.SaveChangesAsync();
        return CreatedAtAction("PostHotelRoom", new { id = hotelroom.Id }, hotelroom);
      }
      return hotelroom;
      
    }

    // DELETE: api/HotelRooms/5
    [HttpDelete("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
    public async Task<IActionResult> DeleteHotelRoom( int hotelId, int roomNumber )
    {
      var hotelRoom = _context.HotelRoom.Where(hotelroom => hotelroom.Hotel.Id == hotelId && hotelroom.Room.Id == roomNumber).FirstOrDefault();
      var hotel = _context.Hotels.Where(hotel => hotel.Id == hotelId).FirstOrDefault();

      //var hotelRoom = await _context.HotelRoom.FindAsync(hotelId);
      if (hotelRoom == null)
      {
        return NotFound();
      }

      _context.HotelRoom.Remove(hotelRoom);
      hotel.HotelRoom.Remove(hotelRoom);

      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool HotelRoomExists( int id )
    {
      return _context.HotelRoom.Any(e => e.Id == id);
    }
  }
}
