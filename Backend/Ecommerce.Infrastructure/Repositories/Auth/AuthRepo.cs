using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Repositories.Auth;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Ecommerce.Infrastructure.Repositories.Auth
{
    public class AuthRepo : IAuthRepo
    {
        private readonly string _cs;

        public AuthRepo(IConfiguration config)
        {
            _cs = config.GetConnectionString("dbcs");
        }

        public async Task ChangePassword(int userId, ChangePasswordDto model)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                SqlCommand cmd = new SqlCommand("usp_ChangePassword", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@OldPassword", model.OldPassword);
                cmd.Parameters.AddWithValue("@NewPassword", model.NewPassword);

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> VerifyEmail(string email)
        {
            using(SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_VerifyEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                var result = await cmd.ExecuteScalarAsync();

                return result != null;

            }
        }        

        public async Task SaveOtpAsync(string email, string otp)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_SaveOtp", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Otp", otp);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            using(SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_VerifyOtp", conn);
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Otp", otp);

                var result = await cmd.ExecuteScalarAsync();

                return result != null;
            }
        }

        public async Task ResetPasswordAsync(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_ResetPassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
