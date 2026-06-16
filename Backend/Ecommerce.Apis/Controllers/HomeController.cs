using Ecommerce.Application.Interfaces.Services.User;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.User;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

      [HttpGet("UserDetails")]
      public async Task<IActionResult> GetUserDetailis(string email)
      {
            var userDetails = await _userService.GetUserDetailis(email);
            return Ok(userDetails);
      }
    }
}
