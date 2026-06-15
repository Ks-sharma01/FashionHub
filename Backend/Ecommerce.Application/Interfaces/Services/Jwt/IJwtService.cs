using Ecommerce.Application.Dtos.User;
using Ecommerce.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Services.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(UserDto user);
    }
}
