using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static AsyncInn.Models.IdentityModelsDTO;
using AsyncInn.Models;

namespace AsyncInn.Controllers.API
{
  [Route("api")]
  [ApiController]
  public class RegisterAndSignin: Controller
  {
    public IActionResult Index()
    {
      return View();
    }
    
  }
}
