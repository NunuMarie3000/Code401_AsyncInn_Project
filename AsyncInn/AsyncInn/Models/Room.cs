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
    public ICollection<Amenity> Amenities { get; set; }
    // Room
    //[ForeignKey("Room")]
    //public Room TheRoom { get; set; }
  }
}
