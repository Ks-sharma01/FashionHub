using Ecommerce.Application.Dtos.Product;
using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Repositories.Product;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ecommerce.Infrastructure.Repositories.Product
{
    public class ProductRepo : IProductRepo
    {
        private readonly string _cs;

        public ProductRepo(IConfiguration config)
        {
            _cs = config.GetConnectionString("dbcs");
        }

        public async Task<ProductDto> AddOrUpdateProduct(ProductDto product, int? id)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_CrudProduct", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                if(id == null || id == 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@Action", "Add"));
                    cmd.Parameters.Add(new SqlParameter("@CreatedAt", DateTime.UtcNow));

                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@Action", "update"));
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    cmd.Parameters.Add(new SqlParameter("@UpdatedAt", DateTime.UtcNow));
                }
                cmd.Parameters.Add(new SqlParameter("@Name", product.Name));
                cmd.Parameters.Add(new SqlParameter("@Description", product.Description));
                cmd.Parameters.Add(new SqlParameter("@Category", product.Category));
                cmd.Parameters.Add(new SqlParameter("@Subcategory", product.Subcategory));
                cmd.Parameters.Add(new SqlParameter("@Prize", product.Prize));
                cmd.Parameters.Add(new SqlParameter("@Stock", product.Stock));
                cmd.Parameters.Add(new SqlParameter("@ImageUrl", product.ImageUrl));


                await cmd.ExecuteNonQueryAsync();

                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Subcategory = product.Subcategory,
                    Description = product.Description,
                    Prize = product.Prize,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl,


                };

            }
        }

        public async Task<List<ProductDto>> GetAllProduct()
        {
            List<ProductDto> products = new List<ProductDto>();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_CrudProduct", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Action", "GetAll"));

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                while (dr.Read())
                {
                    products.Add(new ProductDto
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Category = dr["Category"].ToString(),
                        Description = dr["Description"].ToString(),
                        Subcategory = dr["Subcategory"].ToString(),
                        Prize = Convert.ToDecimal(dr["Prize"]),
                        Stock = Convert.ToInt32(dr["Stock"]),
                        ImageUrl = dr["ImageUrl"].ToString(),

                    });
                }
            }
            return products;
        }

        public async Task<int> PlaceOrder(CreateOrderDto dto)
        {
            int orderId = 0;

            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand("usp_InsertOrder", conn, transaction);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@CustomerName", dto.CustomerName);

                    cmd.Parameters.AddWithValue(
                        "@Email", dto.Email);

                    cmd.Parameters.AddWithValue(
                        "@MobileNo", dto.MobileNo);

                    cmd.Parameters.AddWithValue(
                        "@AddressLine1", dto.AddressLine1);

                    cmd.Parameters.AddWithValue(
                        "@AddressLine2",
                        string.IsNullOrEmpty(dto.AddressLine2)
                        ? null
                        : dto.AddressLine2);

                    cmd.Parameters.AddWithValue(
                        "@City", dto.City);

                    cmd.Parameters.AddWithValue(
                        "@State", dto.State);

                    cmd.Parameters.AddWithValue(
                        "@Pincode", dto.Pincode);

                    cmd.Parameters.AddWithValue(
                        "@TotalAmount", dto.TotalAmount);

                    cmd.Parameters.AddWithValue(
                        "@PaymentMethod", dto.PaymentMethod);

                    cmd.Parameters.AddWithValue(
                        "@PaymentStatus",
                        dto.PaymentMethod == "COD"
                        ? "Pending"
                        : "Paid");

                    cmd.Parameters.AddWithValue(
                        "@OrderStatus", "Pending");

                    orderId = Convert.ToInt32(
                        await cmd.ExecuteScalarAsync());

                    foreach (var item in dto.Items)
                    {
                        
                        SqlCommand itemCmd =
                            new SqlCommand(
                                "usp_InsertOrderItem",
                                conn, transaction);

                        itemCmd.CommandType =
                            CommandType.StoredProcedure;

                        itemCmd.Parameters.AddWithValue(
                            "@OrderId", orderId);

                        itemCmd.Parameters.AddWithValue(
                            "@ProductId", item.ProductId);

                        itemCmd.Parameters.AddWithValue(
                            "@ProductName", item.ProductName);

                        itemCmd.Parameters.AddWithValue(
                            "@Quantity", item.Quantity);

                        itemCmd.Parameters.AddWithValue(
                            "@Price", item.Price);

                        itemCmd.Parameters.AddWithValue(
                            "@UserId", item.UserId);

                        itemCmd.Parameters.AddWithValue(
                            "@Status", item.Status);
                        itemCmd.Parameters.AddWithValue(
                           "@OrderedDate", DateTime.Now);
                        itemCmd.Parameters.AddWithValue(
                           "@IsCancelled", item.IsCancelled);
                        itemCmd.Parameters.AddWithValue(
                           "@PaymentMethod", item.PaymentMethod);
                        itemCmd.Parameters.AddWithValue(
                           "@DeliveredOn", item.DeliveredOn);

                        await itemCmd.ExecuteNonQueryAsync();
                    }

                    transaction.Commit();
                    return orderId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        { 
            ProductDto product = new ProductDto();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_CrudProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "GetById");
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader dr = await cmd.ExecuteReaderAsync();

                if (dr.Read())
                {
                    product = new ProductDto
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Category = dr["Category"].ToString(),
                        Subcategory = dr["Subcategory"].ToString(),
                        Stock = Convert.ToInt32(dr["Stock"]),
                        Prize = Convert.ToInt32(dr["Prize"]),
                        ImageUrl = Convert.ToString(dr["ImageUrl"])
                    };
                }
            }
            return product;
        }

        public async Task<string> DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_CrudProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@Id", id);

                await cmd.ExecuteNonQueryAsync();
                return $"Product having Id = {id} deleted";
            }
        }

        public async Task<List<OrderItemDto>> GetOrderedItems(int userId)
        {
            List<OrderItemDto> orderedItems = new List<OrderItemDto>();
            using(SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_GetOrderedItems", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    orderedItems.Add(new OrderItemDto
                    {
                        OrderId = Convert.ToInt32(dr["OrderId"]),
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        UserId = Convert.ToInt32(dr["UserId"]),
                        Status = Convert.ToString(dr["Status"]),
                        OrderedDate = Convert.ToDateTime(dr["OrderedDate"]),
                        PaymentMethod = dr["PaymentMethod"].ToString(),
                        DeliveredOn = Convert.ToDateTime(dr["DeliveredOn"])
                        
                    });
                }
                
            }
            return orderedItems;
        }

        public async Task AddToCart(AddCartDto cart)
        {
            using(SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_AddToCart", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", cart.UserId);
                cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
                cmd.Parameters.AddWithValue("@Action", cart.Action);

                await cmd.ExecuteNonQueryAsync();


            }
        }

        public async Task<List<GetCartDto>> GetCartItems(int userId)
        {
            List<GetCartDto> cartItems = new List<GetCartDto>();

            using (SqlConnection conn = new SqlConnection(_cs))
            {

             SqlCommand cmd = new SqlCommand("usp_GetCartItems", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            await conn.OpenAsync();

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                cartItems.Add(new GetCartDto
                {
                    CartId = Convert.ToInt32(reader["CartId"]),
                    ProductId = Convert.ToInt32(reader["ProductId"]),
                    ProductName = reader["ProductName"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    Stock = Convert.ToInt32(reader["Stock"]),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"])

                });
            }

            return cartItems;
            }
        }

        public async Task<int> GetCartCount(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_GetCartCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task ClearCartItem(int userId, int productId)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_ClearCartItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                await cmd.ExecuteNonQueryAsync();
            }
        }

            public async Task<List<OrdersWithItemsDto>> GetOrdersWithItems()
            {
            List<OrdersWithItemsDto> orders = new List<OrdersWithItemsDto>();
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_GetOrdersWithItems", conn);
                cmd.CommandType = CommandType.StoredProcedure;
              
                SqlDataReader dr = await cmd.ExecuteReaderAsync();
                while (dr.Read())
                {
                    orders.Add(new OrdersWithItemsDto
                    {
                        UserId = Convert.ToInt32(dr["UserId"]),
                        OrderId = Convert.ToInt32(dr["OrderId"]),
                        CustomerName = Convert.ToString(dr["CustomerName"]),
                        CustomerEmail = Convert.ToString(dr["Email"]),
                        Mobile = Convert.ToString(dr["MobileNo"]),
                        TotalAmount = Convert.ToDecimal(dr["TotalAmount"]),
                        OrderStatus = Convert.ToString(dr["OrderStatus"]),
                        OrderedDate = Convert.ToDateTime(dr["OrderedDate"]),
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"]),
                        PaymentMethod = Convert.ToString(dr["PaymentMethod"]),
                        ItemTotal = Convert.ToDecimal(dr["ItemTotal"]),

                    });
                }
                return orders;
            }
    

            }

        public async Task ChangeOrderStatus(int userId, int orderId, string orderStatus)
        {
            using (SqlConnection conn = new SqlConnection(_cs))
            {
                SqlCommand cmd = new SqlCommand("usp_ChangeOrderStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@Status", orderStatus);
                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            bool result = false;

            using (SqlConnection conn = new SqlConnection(_cs))
            {
                SqlCommand cmd = new SqlCommand("usp_CancelOrder", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);


                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = Convert.ToBoolean(reader["Success"]);
                    }
                }
            }
            return result;
        }

        public async Task<List<ProductDto>> GetProductsPaged(int pageNumber = 1, int pageSize = 10, string search = "", string filterType = "All", string category = "All", string status = "All", DateTime? fromDate = null, DateTime? toDate = null)
        {
            List<ProductDto> products = new List<ProductDto>();

            using (SqlConnection conn = new SqlConnection(_cs))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_GetProductsPaged", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                cmd.Parameters.AddWithValue("@Search", string.IsNullOrEmpty(search) ? "" : search);
                cmd.Parameters.AddWithValue("@FilterType", filterType);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);


                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new ProductDto
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Prize = Convert.ToDecimal(reader["Prize"]),
                        Stock = Convert.ToInt32(reader["Stock"]),
                        Category = reader["Category"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString(),
                        Status = reader["Status"].ToString(),
                        OrderDate = reader["OrderedDate"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["OrderedDate"]),
                                            DeliveredDate = reader["DeliveredOn"] == DBNull.Value
                        ? DateTime.MinValue
                        : Convert.ToDateTime(reader["DeliveredOn"]),
                        PaymentMethod = reader["PaymentMethod"].ToString(),
                        TotalRecords = Convert.ToInt32(reader["TotalRecords"])
                    });
                }
                return products;
            }
        }
    }
}
