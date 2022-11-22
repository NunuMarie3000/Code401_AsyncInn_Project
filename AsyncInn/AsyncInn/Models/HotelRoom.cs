using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
    public class HotelRoom
    {
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public decimal Rate { get; set; }
    public bool PetFriendly { get; set; }
    // Hotel
    [ForeignKey("Hotel")]
    public int HotelID { get; set; }
    public Hotel Hotel { get; set; }
    // Room
    [ForeignKey("Room")]
    public int RoomID { get; set; }
    public Room Room { get; set; }

        public static implicit operator List<object>( HotelRoom v )
        {
            throw new NotImplementedException();
        }
    }
}
