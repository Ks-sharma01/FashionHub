using Ecommerce.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Repositories.Auth
{
    public interface IAuthRepo
    {
        Task ChangePassword(int userId, ChangePasswordDto model);
        Task<bool> VerifyEmail(string email);
        Task SaveOtpAsync(string email, string otp);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task ResetPasswordAsync(string email, string password);
    }
}
