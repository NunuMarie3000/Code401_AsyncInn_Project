using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncInn.Models
{
    public class Hotel
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    // i'm assuming these will be the Navigation properties
    // will add HotelRoom
    //[ForeignKey("HotelRoom")]
    public List<HotelRoom> HotelRoom { get; set; }
    }
}
