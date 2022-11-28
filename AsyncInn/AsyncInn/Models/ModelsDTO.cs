using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
  public class ModelsDTO
  {
    public class DTOAmenity
    {
      public string Name { get; set; }
    }
    public class DTOHotel
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string StreetAddress { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string Country { get; set; }
      public string Phone { get; set; }
      public List<DTOHotelRoom> HotelRooms { get; set; } = new List<DTOHotelRoom>();
    }
    public class DTOHotelRoom
    {
      public int Id { get; set; }
      public int RoomNumber { get; set; }
      public decimal Rate { get; set; }
      public bool PetFriendly { get; set; }
      public List<DTOAmenity> RoomAmenities { get; set; } = new List<DTOAmenity>();
      public int HotelId { get; set; }
      public int RoomId { get; set; }

      //public Hotel Hotel { get; set; }
      //public Room Room { get; set; }
      //public List<DTORoom> RoomList { get; set; } = new List<DTORoom>();
    }
    public class DTORoom
    {
      public string Name { get; set; }
      public int Layout { get; set; }
      public List<DTOAmenity> RoomAmenities { get; set; } = new List<DTOAmenity>();
    }
    
  }
}
