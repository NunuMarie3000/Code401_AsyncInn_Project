using System.ComponentModel.DataAnnotations;

namespace AsyncInn.Models
{
  public class IdentityModelsDTO
  {
    //public class LoginDTO
    //{
    //  [Required]
    //  public string Username { get; set; }
    //  [Required]
    //  public string Password { get; set; }
    //}
    public class RegisterDTO
    {
      [Required]
      public string Username { get; set; }

      [Required]
      public string Password { get; set; }

      [Required]
      public string Email { get; set; }

      public string PhoneNumber { get; set; }
    }

    public class UserDTO
    {
      public string Id { get; set; }
      public string Username { get; set; }
    }
    public class RegisterUserDTO
    {
      [Required]
      public string Username { get; set; }

      [Required]
      public string Password { get; set; }

      [Required]
      public string Email { get; set; }

      public string PhoneNumber { get; set; }
    }
  }
}
