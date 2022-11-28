using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    public async Task<ActionResult<IEnumerable<ModelsDTO.DTOHotel>>> GetHotels()
    {
      var hotels = await _context.Hotels.ToListAsync();
      //var hotelRooms = await _context.HotelRoom.Include(hr => hr.Room.RoomAmenities).ThenInclude(ra => ra.Amenities)..ToListAsync();
      List<ModelsDTO.DTOHotel> allHotels = new List<ModelsDTO.DTOHotel>();
      foreach (var hotel in hotels)
      {
        var associatedHotelRooms = await _context.HotelRoom.Where(hr => hr.HotelID == hotel.Id).Include(r => r.Room).ThenInclude(am=> am.RoomAmenities).ThenInclude(a => a.Amenities).ToListAsync();

        ModelsDTO.DTOHotel hoteldto = new() { Id=hotel.Id, City = hotel.City, Country = hotel.Country, Name = hotel.Name, Phone = hotel.Phone, State = hotel.State, StreetAddress = hotel.StreetAddress };
        allHotels.Add(hoteldto);

        foreach (var hr in associatedHotelRooms)
        {
          ModelsDTO.DTOHotelRoom newHr = new() { PetFriendly = hr.PetFriendly, Rate = hr.Rate, RoomNumber = hr.RoomNumber, Id = hr.Id };
          hoteldto.HotelRooms.Add(newHr);

          foreach (var am in hr.Room.RoomAmenities)
          {
            ModelsDTO.DTOAmenity newAmenity = new() {Name = am.Amenities.Name };
            newHr.RoomAmenities.Add(newAmenity);
          }
        }
      }
      return allHotels;
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ModelsDTO.DTOHotel>> GetHotel( int id )
    {
      var hotel = await _context.Hotels.FindAsync(id);

      // querying all hotelroom objects associated with the given hotel
      // First Include() makes sure it grabs the roomAmenities table and adds it to the data returned to me
      // ThenIncludes() makes sure each roomAmenities also pulls in the individual amenities objects to me
      var hotelrooms = await _context.HotelRoom.Where(hr => hr.HotelID == id).Include(hr => hr.Room.RoomAmenities).ThenInclude(ra => ra.Amenities).ToListAsync();

      if (hotel == null)
      {
        return NotFound();
      }

      // new hoteldto based on input id
      ModelsDTO.DTOHotel hoteldto = new() { City = hotel.City, Country = hotel.Country, Name = hotel.Name, Phone = hotel.Phone, State = hotel.State, StreetAddress = hotel.StreetAddress };
      // hoteldto has a list of hotelRoom objects
      // for each item in hotelroomsquery, 
      // create new hotelroomdto
      // and add that object to the hoteldto's hotelRoom list
      foreach (var item in hotelrooms)
      {
        // creates new instance of HotelRoomDTO
        ModelsDTO.DTOHotelRoom newhotelroomdto = new() { HotelId=item.Id, RoomNumber = item.RoomNumber, Rate = item.Rate, PetFriendly = item.PetFriendly, RoomId=item.RoomID,  Id = item.Id };
        // adds new hotelroomdto to the list inside of hoteldto object
        hoteldto.HotelRooms.Add(newhotelroomdto);

        //while i'm here, each hotelroom object has a reference to the room object, which contains a list of roomAmenities objects, i can create dtos for them while i'm here
        foreach (var i in item.Room.RoomAmenities)
        {
          ModelsDTO.DTOAmenity newAmenity = new() { Name = i.Amenities.Name };
          newhotelroomdto.RoomAmenities.Add(newAmenity);
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
