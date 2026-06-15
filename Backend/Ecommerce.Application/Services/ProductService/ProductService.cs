using Ecommerce.Application.Dtos.Product;
using Ecommerce.Application.Dtos.User;
using Ecommerce.Application.Interfaces.Repositories.Product;
using Ecommerce.Application.Interfaces.Services.Product;
using Ecommerce.Application.Interfaces.User;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ProductDto> AddOrUpdateProduct(ProductDto product, int? id)
        {
            try
            {
                if (product.ImagePath != null)
                {
                    var fileName = Guid.NewGuid().ToString() +
                                   Path.GetExtension(product.ImagePath.FileName);

                    var folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images"
                    );

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImagePath.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/" + fileName;
                }
                else
                {
                    // Keep existing image while updating
                    if (product.Id > 0)
                    {
                        var existingProduct =
                            await _productRepo.GetProductById(product.Id);

                        if (existingProduct != null)
                        {
                            product.ImageUrl = existingProduct.ImageUrl;
                        }
                    }
                }
                return await _productRepo.AddOrUpdateProduct(product, id);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ProductDto>> GetAllProduct()
        {
            try
            {
                return await _productRepo.GetAllProduct();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<int> PlaceOrder(CreateOrderDto dto)
        {
            try
            { 
               
                return await _productRepo.PlaceOrder(dto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public async Task<ProductDto> GetProductById(int id)
        {
            try
            {
                return await _productRepo.GetProductById(id);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<string> DeleteProduct(int id)
        {
            try
            {

              return await _productRepo.DeleteProduct(id);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

       public async Task<List<OrderItemDto>> GetOrderedItems(int userId)
        {
            try
            {
                return await _productRepo.GetOrderedItems(userId);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task AddToCart(AddCartDto cart)
        {
            try
            {

                 await _productRepo.AddToCart(cart);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<GetCartDto>> GetCartItems(int userId)
        {
            try
            {
                return await _productRepo.GetCartItems(userId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<int> GetCartCount(int userId)
        {
            try
            {
                return await _productRepo.GetCartCount(userId);
            }
            catch (Exception )
            {
                throw;
            }
        }

        public async Task ClearCartItem(int userId, int productId)
        {
            try
            {
                await _productRepo.ClearCartItem(userId, productId);    
            }
            catch(Exception )
            {
                throw;
            }
        }

        public async Task<List<OrdersWithItemsDto>> GetOrdersWithItems()
        {
            try
            {
                return await _productRepo.GetOrdersWithItems();
            }
            catch( Exception )
            {
                throw;
            }
        }

        public async Task ChangeOrderStatus(int userId, int orderId, string orderStatus)
        {
            try
            {
                await _productRepo.ChangeOrderStatus(userId, orderId, orderStatus);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            try
            {
                return await _productRepo.CancelOrder(orderId);
            }
            catch (Exception )
            {
                throw;
            }
        }

    }
}
