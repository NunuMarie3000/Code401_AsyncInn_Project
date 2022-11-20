using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
  public class RoomAmenities
  {
    public int Id { get; set; }
    //[ForeignKey("Amenity")]
    public int AmenitiesID { get; set; }
    //[ForeignKey("Room")]
    public int RoomID { get; set; }
    // Amenities
    [ForeignKey("Amenity")]
    public ICollection<Amenity> Amenities { get; set; }
    // Room
    //[ForeignKey("Room")]
    public Room Room { get; set; }
  }
}
