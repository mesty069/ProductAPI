using Microsoft.EntityFrameworkCore;
using Product.Core.Entities.Order;
using Product.Core.Interface;
using Product.Core.Services;
using Product.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uOW;
        private readonly ApplicationDbContext _context;

        public OrderServices(IUnitOfWork UOW, ApplicationDbContext context)
        {
            _uOW = UOW;
            _context = context;
        }

        //public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMehthodId, string basketId, ShipAddress shipAddress)
        //{
        //    var basket = await _uOW.BasketRepository.GetBasketAsync(basketId);
        //    var items = new List<OrderItems>();
        //    foreach (var item in basket.BasketItems)
        //    {
        //        var productItem = await _uOW.ProductRepository.GetByIdAsync(item.Id);
        //        var productItemOrderd = new ProductItemOrderd(productItem.Id, productItem.Name, productItem.ProductPicture);
        //        var orderItem = new OrderItems(productItemOrderd, item.Price, item.Quantity);
        //        items.Add(orderItem);
        //    }
        //    await _context.OrderItems.AddRangeAsync(items);
        //    var deliverymethod = await _context.DeliveryMethods.Where(x => x.Id == deliveryMehthodId).FirstOrDefaultAsync();
        //    var subtotal = items.Sum(x => x.Price * x.Quantity);
        //    var order = new Order(buyerEmail, shipAddress, deliverymethod, items, subtotal);
        //    if (order is null) return null;
        //    //新增下單資料
        //    await _context.Orders.AddAsync(order);
        //    await _context.SaveChangesAsync();
        //    //刪除購物籃資料
        //    await _uOW.BasketRepository.DeleteBasketAsync(basketId);
        //    return order;



        //}

        ///// <summary>
        ///// 查詢所有配送方式
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        //=> await _context.DeliveryMethods.ToListAsync();

        ///// <summary>
        ///// 查詢用戶訂單詳情
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="buyerEmail"></param>
        ///// <returns></returns>
        //public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        //{
        //    var order = await _context.Orders
        //        .Where(x => x.OrderId == id && x.BuyerEmail == buyerEmail)
        //        .Include(x => x.OrderItems).ThenInclude(x => x.productItemOrderd)
        //        .Include(x => x.DeliveryMethod).FirstOrDefaultAsync();
        //    return order;
        //}

        ///// <summary>
        ///// 查詢用戶訂單清單
        ///// </summary>
        ///// <param name="buyerEmail"></param>
        ///// <returns></returns>
        //public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        //{
        //    var order = await _context.Orders
        //        .Where(x => x.BuyerEmail == buyerEmail)
        //        .Include(x => x.OrderItems).ThenInclude(x => x.productItemOrderd)
        //        .Include(x => x.DeliveryMethod)
        //        .OrderByDescending(x => x.OrderDate)
        //        .ToListAsync();
        //    return order;
        //}
    }
}
