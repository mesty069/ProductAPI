using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Product.Core.Entities.Order
{
    public class Order : BaseEntity<int>
    {
        public Order() { }

        public Order(string buyerEmail, ShipAddress shipAddress, DeliveryMethod deliverymethod, List<OrderItems> orderItems, int? subtotal, string? paymentIntentId = null)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipAddress;
            DeliveryMethod = deliverymethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }

        public int OrderId { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ShipAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItems> OrderItems { get; set; }
        public int? Subtotal { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }

        public int GetTotal()
        {
            return (Subtotal.HasValue? Subtotal.Value : 0 )+ DeliveryMethod.Price;
        }
    }
}
