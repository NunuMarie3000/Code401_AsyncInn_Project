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
    public async Task<ActionResult<IEnumerable<ModelsDTO.DTOHotelRoom>>> GetHotelRoom( int hotelId )
    {
      var hotelRooms =  await _context.HotelRoom.Where(hotelroom => hotelroom.HotelID == hotelId).Include(hr=> hr.Room).ThenInclude(r=> r.RoomAmenities).Include(hr=> hr.Room.RoomAmenities).ToListAsync();
      List<ModelsDTO.DTOHotelRoom> result = new List<ModelsDTO.DTOHotelRoom>();

      foreach (var hr in hotelRooms)
      {
        ModelsDTO.DTOHotelRoom newHR = new ModelsDTO.DTOHotelRoom() { Id = hr.Id, PetFriendly = hr.PetFriendly, Rate = hr.Rate, RoomNumber = hr.RoomNumber };
        foreach (var ra in hr.Room.RoomAmenities)
        {
          var amenity = _context.Amenities.Where(a => a.Id == ra.AmenitiesID).FirstOrDefault();
          if (amenity == null)
            return BadRequest();

          ModelsDTO.DTOAmenity newAmenity = new ModelsDTO.DTOAmenity() { Name = amenity.Name };
          newHR.RoomAmenities.Add(newAmenity);
        }
        result.Add(newHR);
      }

      return result;
    }

    [HttpPost]
    public async Task<ActionResult<HotelRoom>> AddRoomToAHotel( int hotelId, Room room )
    {
      // find the hotel in the context
      var hotel = await _context.Hotels.Where(h => h.Id == hotelId).FirstOrDefaultAsync();
      if (hotel == null)
      {
        return NotFound();
      }
      // create new HotelRoom object with reference to hotelid and the roomid
      HotelRoom newHotelRoom = new HotelRoom() { HotelID = hotelId, RoomID = room.Id };
      // save hotelroom to hotel's hotelroom list
      hotel.HotelRoom.Add(newHotelRoom);
      // add newhotelroom to the context
      _context.HotelRoom.Add(newHotelRoom);
      // save context
      await _context.SaveChangesAsync();
      // redirect to Index action
      return RedirectToAction("Index");
    }

    // GET: api/HotelRooms/5
    //[HttpGet("{id}")]
    [HttpGet("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
    public async Task<ActionResult<HotelRoom>> GetHotelRoom( int hotelId, int roomNumber )
    {
      var hotelRoom = await _context.HotelRoom.Where(hotelroom => hotelroom.Hotel.Id == hotelId && hotelroom.RoomNumber == roomNumber).Include(hr => hr.Room).ThenInclude(r => r.RoomAmenities).FirstOrDefaultAsync();

      if (hotelRoom == null)
      {
        return NotFound();
      }
      return hotelRoom;
    }

    // PUT: api/HotelRooms/5
    // update the details of a specific room
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("/api/Hotels/{hotelId}/Rooms/{roomNumber}")]
    public async Task<IActionResult> PutHotelRoom( int hotelId, int roomNumber, HotelRoom updatedHotelRoom )
    {
      //var chosen = _context.HotelRoom.Where(hotelroom => hotelroom.HotelID == hotelId && hotelroom.RoomID == roomNumber).FirstOrDefault();
      if (hotelId != updatedHotelRoom.Id)
      {
        return BadRequest();
      }

      var currentHotelRoom = _context.HotelRoom.Where(hr => hr.Id == updatedHotelRoom.Id).FirstOrDefault();
      _context.HotelRoom.Remove(currentHotelRoom);

      var hotel = _context.Hotels.Where(h => h.Id == hotelId).FirstOrDefault();
      if (hotel == null)
      {
        return NotFound();
      }

      for (int i = 0; i <= hotel.HotelRoom.Count; i++)
      {
        if (hotel.HotelRoom[ i ].Id == updatedHotelRoom.Id)
        {
          hotel.HotelRoom[ i ] = updatedHotelRoom;
        }
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
    public async Task<ActionResult<HotelRoom>> PostHotelRoom( int hotelId, [Bind("Id, HotelId, RoomId, RoomNumber, Rate, PetFriendly")] HotelRoom hotelroom )
    {
      // add a room to a hotel
      var hotel = _context.Hotels.Where(h => h.Id == hotelId).FirstOrDefault();

      // we need to add the hotelRoom to this, but where are we getting the hotelRoom object from?
      // i guess from the body? do we have to do that bind thing i keep seeing?
      //_context.HotelRoom.Add(hotelRoom);
      if (ModelState.IsValid)
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
      var hotelRoom = _context.HotelRoom.Where(hotelroom => hotelroom.HotelID == hotelId && hotelroom.RoomID == roomNumber).FirstOrDefault();
      var hotel = _context.Hotels.Where(hotel => hotel.Id == hotelId).FirstOrDefault();

      //var hotelRoom = await _context.HotelRoom.FindAsync(hotelId);
      if (hotelRoom == null || hotel == null)
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
