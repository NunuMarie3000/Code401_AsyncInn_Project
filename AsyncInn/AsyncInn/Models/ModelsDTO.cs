using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
  public class ModelsDTO
  {
    public class AmenityDTO
    {
      public string Name { get; set; }
    }
    public class HotelDTO
    {
      public string Name { get; set; }
      public string StreetAddress { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string Country { get; set; }
      public string Phone { get; set; }
      public List<HotelRoomDTO> Rooms { get; set; } = new List<HotelRoomDTO>();
    }
    public class HotelRoomDTO
    {
      public int RoomNumber { get; set; }
      public decimal Rate { get; set; }
      public bool PetFriendly { get; set; }

      public Hotel Hotel { get; set; }
      public Room Room { get; set; }
      public List<Room> room { get; set; } = new List<Room>();
    }
    public class RoomDTO
    {
      public string Name { get; set; }
      public int Layout { get; set; }
      public List<RoomAmenities> RoomAmenities { get; set; }
    }
    
  }
}
