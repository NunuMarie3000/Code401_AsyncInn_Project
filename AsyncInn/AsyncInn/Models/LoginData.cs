using System.ComponentModel.DataAnnotations;

namespace AsyncInn.Models
{
  // this will be inbound DTO for user input
    public class LoginData
    {
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }
    }
}
