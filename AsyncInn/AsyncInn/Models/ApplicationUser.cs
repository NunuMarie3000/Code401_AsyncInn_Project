using Azure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AsyncInn.Models
{
  public class ApplicationUser: IdentityUser
  {

    private static UserManager<ApplicationUser> userManager;

    public ApplicationUser() { }

    public ApplicationUser( UserManager<ApplicationUser> manager )
    {
      userManager = manager;
    }

    public static async Task<ApplicationUser> Register( IdentityModelsDTO.RegisterUserDTO data )
    {
      // throw new NotImplementedException();
      var user = new ApplicationUser()
      {
        UserName = data.Username,
        Email = data.Email,
        PhoneNumber = data.PhoneNumber
      };

      var result = await userManager.CreateAsync(user, data.Password);

      if (result.Succeeded)
      {
        return user;
      }
      return null;
    }
  }
}