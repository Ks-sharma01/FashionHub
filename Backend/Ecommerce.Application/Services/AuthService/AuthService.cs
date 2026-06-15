using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Repositories.Auth;
using Ecommerce.Application.Interfaces.Services.Auth;
using Ecommerce.Application.Interfaces.Services.Jwt;
using Ecommerce.Application.Interfaces.Services.Otp;
using Ecommerce.Application.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _repository;
        private readonly IJwtService _jwtService;
        private readonly IAuthRepo _authRepo;
        private readonly IOtpService _otpService;

        public AuthService(
            IUserRepo repository,
            IJwtService jwtService, IAuthRepo authRepo, IOtpService otpService)
        {
            _repository = repository;
            _jwtService = jwtService;
            _authRepo = authRepo;
            _otpService = otpService;
        }

        public async Task<string> Login(LoginDto model)
        {
            var user = await _repository.Login(
                model.Email,
                model.Password);

            if (user == null)
                return null;

            return _jwtService.GenerateToken(user);
        }

        public async Task ChangePassword(int userId, ChangePasswordDto model)
        {
            try
            {
                if(model.NewPassword != model.ConfirmPassword)
                {
                    throw new Exception("New Password and Confirm Password do not match.");
                }
                await _authRepo.ChangePassword(userId, model);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task VerifyEmail(string email)
        {
            try
            {
                bool isEmailExists = await _authRepo.VerifyEmail(email);
                if (!isEmailExists)
                    throw new Exception("Email not registered");

                string otp = new Random().Next(100000, 999999).ToString();

                await _authRepo.SaveOtpAsync(email, otp);

                await _otpService.SendOtpEmailAsync(email, otp);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task VerifyOtpAsync(string email,string otp)
        {
            bool isValid = await _authRepo.VerifyOtpAsync(email, otp);

            if (!isValid)
                throw new Exception("Invalid or Expired OTP");
        }

        public async Task ResetPasswordAsync(string email, string password)
        {

            await _authRepo.ResetPasswordAsync(email,password);
        }
    }
}
