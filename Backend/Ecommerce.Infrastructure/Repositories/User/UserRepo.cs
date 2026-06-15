using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.User;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.User;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories.User
{
    public class UserRepo : IUserRepo
    {
        private readonly string _cs;

        public UserRepo(IConfiguration config)
        {
            _cs = config.GetConnectionString("dbcs");
        }

        public async Task<UserDto> AddOrUpdateUser(UserDto user)
        {
            
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_CrudUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if(user.Id == null || user.Id == 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@Action", "insert"));
                    cmd.Parameters.Add(new SqlParameter("@CreatedAt", DateTime.UtcNow));

                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@Action", "update"));
                    cmd.Parameters.Add(new SqlParameter("@UpdatedAt", DateTime.UtcNow));

                }
                cmd.Parameters.Add(new SqlParameter("@Name", user.Name));
                cmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                cmd.Parameters.Add(new SqlParameter("@Password", user.Password));
                cmd.Parameters.Add(new SqlParameter("@Age", user.Age));
                cmd.Parameters.Add(new SqlParameter("@Status", user.Status));

                var result = await cmd.ExecuteScalarAsync();
                string message = result?.ToString();

                return new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Age = user.Age,
                    Message = message

                };

            }
        }

        public async Task<List<UserDto>> GetAllUsers(UserDto user)
        {
            List<UserDto> users = new List<UserDto>();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_CrudUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Action", "GetAllUsers"));

                SqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    users.Add(new UserDto
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Email = Convert.ToString(dr["Email"]),
                        Status = Convert.ToBoolean(dr["Status"]),
                        Age = Convert.ToInt32(dr["Age"]),
                        Role = dr["Role"].ToString()
                    });
                }
            }
            return users;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var userById = new UserDto();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_CrudUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Action", "GetById"));
                cmd.Parameters.Add(new SqlParameter("@Action", id));

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    userById = new UserDto
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Email = Convert.ToString(dr["Email"]),
                        Status = Convert.ToBoolean(dr["Status"]),
                        Age = Convert.ToInt32(dr["Age"]),
                        Role = dr["Role"].ToString()
                    };
                }
            }
            return userById;
        }


        public async Task<UserDto> Login(string email, string password)
        {
            UserDto userDto = new UserDto();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_Login", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Email", email));
                cmd.Parameters.Add(new SqlParameter("@Password", password));

               SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if(dr.Read())
                {
                    userDto = new UserDto
                    {
                        Id = Convert.ToInt32((dr["Id"])),
                        Email = dr["Email"].ToString(),
                        Name = dr["Name"].ToString(),
                        Password = dr["Password"].ToString(),
                        Role = dr["Role"].ToString()
                    };
                }
                return userDto;
            }
        }


        public async Task<tbl_Orders> GetUserDetailis(string email)
        {
            tbl_Orders userDto = new tbl_Orders();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_GetUserDetails", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Email", email));

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.Read())
                {
                    userDto = new tbl_Orders
                    {
                        Id = Convert.ToInt32((dr["Id"])),
                        Email = dr["Email"].ToString(),
                        CustomerName = dr["CustomerName"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        AddressLine1 = dr["AddressLine1"].ToString(),
                        AddressLine2 = dr["AddressLine1"].ToString() ?? "",
                        City = dr["City"].ToString(),
                        State = dr["State"].ToString(),
                        Pincode = dr["Pincode"].ToString(),
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                        PaymentMethod = dr["PaymentMethod"].ToString(),
                        PaymentStatus = dr["PaymentStatus"].ToString(),
                        OrderStatus = dr["OrderStatus"].ToString(),
                        OrderDate = Convert.ToDateTime(dr["OrderDate"])
                    };
                }
                return userDto;
            }
        }
    }
}
