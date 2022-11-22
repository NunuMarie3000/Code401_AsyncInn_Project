using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
  public class Room
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Layout { get; set; }
    // i'm assuming these will be the Navigation properties
    // Amenities
    //[ForeignKey("Amenity")]
    //public List<Amenity> Amenities { get; set; } = new List<Amenity>();
    // Room
    //[ForeignKey("Room")]
    //public Room TheRoom { get; set; }
    //[ForeignKey("RoomAmenities")]
    public List<RoomAmenities> RoomAmenities { get; set; }
  }
}
