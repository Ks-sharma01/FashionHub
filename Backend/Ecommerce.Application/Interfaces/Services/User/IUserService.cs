using Ecommerce.Application.Dtos.User;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<UserDto> AddOrUpdateUser(UserDto user);
        Task<UserDto> Login(string email, string password);
        Task<tbl_Orders> GetUserDetailis(string email);
    }
}
