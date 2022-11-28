using AsyncInn.Models;
using Microsoft.AspNetCore.Mvc;
using static AsyncInn.Models.IdentityModelsDTO;

namespace AsyncInn.Controllers.API
{
  public class UsersController: Controller
  {
    //public IActionResult Index()
    //{
    //  return View();
    //}

    [HttpGet("register")]
    public ViewResult Register()
    {
      return View();
    }

    //Add an empty controller and add the /register and /signin routes, using your new service
    [Route("register")]
    [HttpPost("register")]
    public async Task<ApplicationUser> Register( RegisterUserDTO data )
    {

      RegisterUserDTO dummydata = new RegisterUserDTO() { Email = "email@email.com", Password = "password", PhoneNumber = "555-555-5555", Username = "userName01" };

      return await ApplicationUser.Register(dummydata);

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
    public Task<UserDTO> Authenticate( string username, string password )
    {
      throw new NotImplementedException();
    }
  }
}
