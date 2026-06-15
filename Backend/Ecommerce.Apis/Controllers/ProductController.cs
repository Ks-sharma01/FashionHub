using Ecommerce.Application.Dtos.Product;
using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Services.Product;
using Ecommerce.Application.Interfaces.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Product/{id?}")]
        public async Task<IActionResult> AddProduct(ProductDto product, int? id)
        { 
            var products = await _productService.AddOrUpdateProduct(product, id);
            return Ok(products);

        }

        [HttpGet("AllProducts")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllProducts()
        
        {        
            var products = await _productService.GetAllProduct();
            return Ok(products);
        }

        [Authorize(Roles = "User")]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                foreach (var item in dto.Items)
                {
                    item.UserId = userId;
                    item.Status = "Placed";
                    item.IsCancelled = item.IsCancelled ? item.IsCancelled : false;
                    item.OrderedDate = DateTime.Now;
                    item.PaymentMethod = dto.PaymentMethod;
                    item.DeliveredOn = DateTime.Now.AddDays(3);
                }
                int orderId = await _productService.PlaceOrder(dto);
                return Ok(new
                {
                    OrderId = orderId,
                    Message = "Order placed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

       [Authorize(Roles = "Admin")]
       [HttpGet("ProductById/{id}")]
       public async Task<IActionResult> GetProductById(int id)
       {     
           var product = await _productService.GetProductById(id); 
           return Ok(product);     
       }

       [Authorize(Roles = "Admin")]
       [HttpPost("DeleteProduct/{id}")] 
       public async Task<IActionResult> DeleteProduct(int id)
       {
          var message = await _productService.DeleteProduct(id);
          return Ok(new
          {
              Message = message
          });
       }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("GetOrderedItems")]
        public async Task<IActionResult> GetOrderedItems(int userId)
        {
            var orderedItems = await _productService.GetOrderedItems(userId);
            return Ok(orderedItems);
        }

       [Authorize(Roles = "User")]
       [HttpPost("Addtocart")]
       public async Task<IActionResult> AddToCart(AddCartDto cart)
        {
            try
            {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if(userId != 0)
            {
                cart.UserId = userId;
            }
            await _productService.AddToCart(cart);
            return Ok(new
            {
                cart = cart,
                message = "Product added to cart"
            });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetCartDetails")]
        public async Task<IActionResult> GetCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _productService.GetCartItems(userId);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetCartCount")]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var count = await _productService.GetCartCount(userId);

            return Ok(count);
        }

        [Authorize(Roles = "User")]
        [HttpPost("ClearCartItem")]
        public async Task<IActionResult> ClearCartItem(int productId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _productService.ClearCartItem(userId, productId);
                return Ok(new
                {
                    messsage = $"Product having Id {productId} has removed from cart"
                });
            }
            catch(Exception ex)
            {
               return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("OrdersWithItems")]
        public async Task<IActionResult> GetOrdersWithItems()
        {
            try
            {
                var orders = await _productService.GetOrdersWithItems();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                
                message = ex.Message.ToString()
                });

            }
        }

        [Authorize(Roles = "Admin")]    
        [HttpPost("ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(int userId, int orderId, string orderStatus)
        {
            try
            {

                await _productService.ChangeOrderStatus(userId, orderId, orderStatus);
                return Ok(new
                {
                    message = "Order status changed"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message.ToString()
                });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var result = await _productService.CancelOrder(orderId);
                if(result == true)
                {
                    return Ok(new
                    {
                        message = "Order cancelled successfully"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "Order cannot be cancelled"

                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ProductsPaged")]
        public async Task<IActionResult> GetProductsPaged(int pageNumber, int pageSize, string search, string filterType)
        {
            try
            {
                var filteredProducts = await _productService.GetProductsPaged(pageNumber, pageSize, search, filterType);
                return Ok(filteredProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message.ToString()
                });
            }
        }
    }
}
