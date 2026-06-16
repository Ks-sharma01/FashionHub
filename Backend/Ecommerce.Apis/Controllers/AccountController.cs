using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Services.Email;
using Ecommerce.Application.Interfaces.Services.User;
using Ecommerce.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> AddOrUpdateUser([FromBody] UserDto user)
        {
           if(user.Id == null || user.Id == 0)
            {
                var password = _emailService.GeneratePassword();
                try
                {
                    await _emailService.SendPasswordEmailAsync(user.Email, password);
                    user.Password = password;
                    var users = await _userService.AddOrUpdateUser(user);
                    return Ok(new
                    {
                        success = true,
                        message = users.Message,
                        data = users
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Failed to send Password email");
                }
            }
           return BadRequest();
            
      
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var users = await _userService.Login(loginDto.Email, loginDto.Password);
            return Ok(users);
        }
    }
}
