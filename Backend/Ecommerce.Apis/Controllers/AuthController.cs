using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Ecommerce.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);

            if (token == null)
                return Unauthorized();

            return Ok(new
            {
                Token = token
            });
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if(userId == 0)
            {
                return Unauthorized(new
                {
                    status = "Failed",
                    message = "Please login"
                });
            }
            await _authService.ChangePassword(userId, model);
            return Ok(new
            {
                status = "Success",
                message = "Password changed successfully"
            });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] ForgetPasswordDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Email))
                    return BadRequest(new
                    {
                        message = "Email not registered"
                    });

                 await _authService.VerifyEmail(dto.Email);

                return Ok(new
                {
                    Message = "Email verified successfully"
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto model)
        {
            try
            {
                await _authService.VerifyOtpAsync(model.Email, model.Otp);

                return Ok(new
                {
                    message = "OTP verified successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            try
            {
                await _authService.ResetPasswordAsync(model.Email, model.Password);

                return Ok(new
                {
                    message = "Password updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
