using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Data.Enums;
using eShopSolution.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Sales
{
    public class OrderService : IOrderService
    {
        private readonly eShopDbContext _context;
        
        public OrderService(eShopDbContext context)
        {
            _context = context;            
        }
        public async Task<int> Create(CheckoutRequest request)
        {
            var orderDetails = new List<OrderDetail>();
            foreach (var item in request.OrderDetailViewModel)
            {
                orderDetails.Add(new OrderDetail()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }            

            var order = new Order()
            {
                UserId = request.UserId != null ? request.UserId: Guid.NewGuid(),
                ShipName = request.Name,
                ShipAddress = request.Address,
                ShipEmail = request.Email,
                ShipPhoneNumber = request.PhoneNumber,
                OrderDate = DateTime.Now,
                Status = request.UserId != null? OrderStatus.InProgress : OrderStatus.Pending, //
                OrderDetails = orderDetails
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;

        }
    }
}
