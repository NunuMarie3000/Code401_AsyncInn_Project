using AsyncInn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static AsyncInn.Models.IdentityModelsDTO;

namespace AsyncInn.Controllers.API
{
  [Route("api/[controller]")]
  //[ApiController]
  public class UsersController: Controller
  {
    [HttpGet]
    public string Index()
    {
      //return View();
      return "This is the usersController index method";
    }

    private UserManager<ApplicationUser> userManager;

    public UsersController( UserManager<ApplicationUser> manager )
    {
      userManager = manager;
    }


    [HttpGet("register")]
    public ViewResult Register()
    {
      return View();
    }

    //Add an empty controller and add the /register and /signin routes, using your new service
    [Route("register")]
    [HttpPost("register")]
    public async Task<UserDTO> Register( [FromBody] RegisterUserDTO data, ModelStateDictionary modelState )
    {
      var user = new ApplicationUser
      {
        UserName = data.Username,
        Email = data.Email,
        PhoneNumber = data.PhoneNumber
      };

      var result = await userManager.CreateAsync(user, data.Password);

      foreach (var error in result.Errors)
      {
        var errorKey =
          error.Code.Contains("Password") ? nameof(data.Password) :
          error.Code.Contains("Email") ? nameof(data.Email) :
          error.Code.Contains("UserName") ? nameof(data.Username) :
          "";
        modelState.AddModelError(errorKey, error.Description);
      }

      if (result.Succeeded)
        return new UserDTO { Username = user.UserName };

      return null;

      //RegisterUserDTO dummydata = new RegisterUserDTO() { Email = "email@email.com", Password = "password", PhoneNumber = "555-555-5555", Username = "userName01" };

      //return await ApplicationUser.Register(dummydata);

      //return await ApplicationUser.Register(data);

      //throw new NotImplementedException();
    }


    [HttpGet("signin")]
    public ViewResult Authenticate()
    {
      return View();
    }

    [Route("signin")]
    [HttpPost("signin")]
    public async Task<ApplicationUser> Login( [FromBody] string username, [FromBody] string password ) //formerly known as Authenticate
    {
      //var user = await userManager.FindByNameAsync(username);
      var userUsername = username;
      var user = await userManager.FindByNameAsync(username);

      if (!ModelState.IsValid)
      {
        return null;
      }
      if (user == null)
      {
        return null;
      }
      var userdto = new UserDTO { Id=user.Id, Username=user.UserName };

      return user;
      //throw new NotImplementedException();
    }
  }
}
