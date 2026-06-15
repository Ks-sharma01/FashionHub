using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Services.User;
using Ecommerce.Application.Interfaces.User;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDto> AddOrUpdateUser(UserDto user)
        {
            try
            {

            return await _userRepo.AddOrUpdateUser(user);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<UserDto> Login(string email, string password)
        {
            try
            {

            return await _userRepo.Login(email, password);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<tbl_Orders> GetUserDetailis(string email)
        {
            try
            {
                return await _userRepo.GetUserDetailis(email);
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
