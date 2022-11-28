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
  public class RoomsController: ControllerBase
  {
    private readonly AsyncInnDbContext _context;

    public RoomsController( AsyncInnDbContext context )
    {
      _context = context;
    }

    //GET: api/Rooms
   [ HttpGet ]
    public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    {
      return await _context.Rooms.ToListAsync();
    }

    //GET: api/Rooms/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> GetRoom( int id )
    {
      var room = await _context.Rooms.FindAsync(id);

      if (room == null)
      {
        return NotFound();
      }

      return room;
    }

    //PUT: api/Rooms/5
     //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom( int id, Room room )
    {
      if (id != room.Id)
      {
        return BadRequest();
      }

      _context.Entry(room).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!RoomExists(id))
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

    //POST: api/Rooms
    //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom( Room room )
    {
      _context.Rooms.Add(room);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetRoom", new { id = room.Id }, room);
    }

    //DELETE: api/Rooms/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom( int id )
    {
      var room = await _context.Rooms.FindAsync(id);
      if (room == null)
      {
        return NotFound();
      }

      _context.Rooms.Remove(room);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool RoomExists( int id )
    {
      return _context.Rooms.Any(e => e.Id == id);
    }

    // add amenities to a specific room
    [Route("{roomId}/Amenity/{amenityId}")]
    [HttpPost]
    public async Task<ActionResult<Room>> PostAmenityToRoom( int roomId, int amenityId )
    {
      var room = _context.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
      RoomAmenities newAmenity = new RoomAmenities() {AmenitiesID = amenityId, RoomID=roomId };
      room.RoomAmenities.Add(newAmenity);
      _context.RoomAmenities.Add(newAmenity);
      await _context.SaveChangesAsync();

      //return CreatedAtAction("GetRoom", new { id = room.Id }, room);
      return Ok();
    }

    // add amenities to a specific room
    [Route("{roomId}/Amenity/{amenityId}")]
    [HttpDelete]
    public async Task<ActionResult<Room>> DeleteAmenityFromRoom( int roomId, int amenityId )
    {
      var room = _context.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
      var roomAmenity = _context.RoomAmenities.Where(ra => ra.AmenitiesID == amenityId && ra.RoomID== roomId).FirstOrDefault();
      room.RoomAmenities.Remove(roomAmenity);
      _context.RoomAmenities.Remove(roomAmenity);
      await _context.SaveChangesAsync();

      //return CreatedAtAction("GetRoom", new { id = room.Id }, room);
      return Ok();
    }
  }
}
