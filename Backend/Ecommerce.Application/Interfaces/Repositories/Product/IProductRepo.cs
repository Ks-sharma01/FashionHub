using Ecommerce.Application.Dtos.Product;
using Ecommerce.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces.Repositories.Product
{
    public interface IProductRepo
    {
        Task<ProductDto> AddOrUpdateProduct(ProductDto product, int? id);
        Task<List<ProductDto>> GetAllProduct();
        Task<int> PlaceOrder(CreateOrderDto dto);
        Task<ProductDto> GetProductById(int id);
        Task<string> DeleteProduct(int id);
        Task<List<OrderItemDto>> GetOrderedItems(int userId);
        Task AddToCart(AddCartDto cart);
        Task<List<GetCartDto>> GetCartItems(int userId);
        Task<int> GetCartCount(int userId);
        Task ClearCartItem(int userId, int productId);
        Task<List<OrdersWithItemsDto>> GetOrdersWithItems();
        Task ChangeOrderStatus(int userId, int orderId, string status);
        Task<bool> CancelOrder(int orderId);
        Task<List<ProductDto>> GetProductsPaged(int pageNumber, int pageSize, string filterType, string category, string status, string search, DateTime? fromDate, DateTime? toDate);
    }
}
