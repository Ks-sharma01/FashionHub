using Ecommerce.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto model);
        Task ChangePassword(int userId, ChangePasswordDto model);
        Task VerifyEmail(string email);
        Task VerifyOtpAsync(string email, string otp);
        Task ResetPasswordAsync(string email, string password);
    }
}
