using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;

namespace AsyncInn.Controllers.Web
{
  public class RoomsController: Controller
  {
    private readonly AsyncInnDbContext _context;

    public RoomsController( AsyncInnDbContext context )
    {
      _context = context;
    }

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
      return View(await _context.Rooms.Include(r => r.RoomAmenities).ToListAsync());
    }

    // GET: Rooms/Details/5
    public async Task<IActionResult> Details( int? id )
    {
      if (id == null || _context.Rooms == null)
      {
        return NotFound();
      }

      var room = await _context.Rooms
          .FirstOrDefaultAsync(m => m.Id == id);
      if (room == null)
      {
        return NotFound();
      }

      return View(await _context.Rooms.Where(r => r.Id == id).Include(r => r.RoomAmenities).ToListAsync());
    }

    // GET: Rooms/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Rooms/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( [Bind("Id,Name,Layout")] Room room )
    {
      if (ModelState.IsValid)
      {
        // when we create a new room, we also need to create a new HotelRoom object, so we need to know what hotel this room is in...
        //_context.Add(room);
        //await _context.SaveChangesAsync();
        //return RedirectToAction(nameof(Index));

        _context.Add(room);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(room);
    }

    // GET: Rooms/Edit/5
    public async Task<IActionResult> Edit( int? id )
    {
      if (id == null || _context.Rooms == null)
      {
        return NotFound();
      }

      var room = await _context.Rooms.FindAsync(id);
      if (room == null)
      {
        return NotFound();
      }
      return View(room);
    }

    // POST: Rooms/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit( int id, [Bind("Id,Name,Layout")] Room room )
    {
      if (id != room.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(room);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!RoomExists(room.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index));
      }
      return View(room);
    }

    // GET: Rooms/Delete/5
    public async Task<IActionResult> Delete( int? id )
    {
      if (id == null || _context.Rooms == null)
      {
        return NotFound();
      }

      var room = await _context.Rooms
          .FirstOrDefaultAsync(m => m.Id == id);
      if (room == null)
      {
        return NotFound();
      }

      return View(room);
    }

    // POST: Rooms/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed( int id )
    {
      if (_context.Rooms == null)
      {
        return Problem("Entity set 'AsyncInnDbContext.Rooms'  is null.");
      }
      var room = await _context.Rooms.FindAsync(id);
      if (room != null)
      {
        _context.Rooms.Remove(room);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool RoomExists( int id )
    {
      return _context.Rooms.Any(e => e.Id == id);
    }

    // POST: [Route("{roomId}/Amenity/{amenityId}")]
    [HttpPost, ActionName("Post")]
    [Route("{roomId}/Amenity/{amenityId}")]
    public async Task<IActionResult> AddAmenityToRoom( int roomId, int amenityId )
    {
      // room that the user wants to add an amenity to
      var inputRoom = _context.Rooms.Where(room => room.Id == roomId).FirstOrDefault();
      // rn, room doesn't have a property to contain amenities...
      // but when we do, we need to use the input Id to add it to the room
      if (ModelState.IsValid)
      {
        try
        {
          // new room amenties object with ref roomID and amentityID
          RoomAmenities newRoomAmenities = new RoomAmenities {RoomID=roomId, AmenitiesID=amenityId };
          // add roomAmenities to room list
          inputRoom.RoomAmenities.Add(newRoomAmenities);
          _context.Entry(newRoomAmenities).State = EntityState.Added;
          // EntityState.Deleted to remove, then save
          // save to db
          await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
      // redirect
      return RedirectToAction(nameof(Index));
    }

    // POST: [Route("{roomId}/Amenity/{amenityId}")]
    [HttpDelete, ActionName("Delete")]
    [Route("{roomId}/Amenity/{amenityId}")]
    public async Task<IActionResult> RemoveAmenityFromRoom( int roomId, int amenityId )
    {
      // room that the user wants to add an amenity to
      var inputRoom = _context.Rooms.Where(room => room.Id == roomId).FirstOrDefault();
      // rn, room doesn't have a property to contain amenities...
      // but when we do, we need to use the input Id to remove the given amenityId
      if (ModelState.IsValid)
      {
        try
        {
          // new room amenties object with ref roomID and amentityID
          RoomAmenities newRoomAmenities = new RoomAmenities { RoomID = roomId, AmenitiesID = amenityId };
          // remove roomAmenities to room list
          inputRoom.RoomAmenities.Remove(newRoomAmenities);
          _context.Entry(newRoomAmenities).State = EntityState.Deleted;
          // EntityState.Deleted to remove, then save
          // save to db
          await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
      // redirect
      return RedirectToAction(nameof(Index));
    }
  }
}
