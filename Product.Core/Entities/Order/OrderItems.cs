namespace Product.Core.Entities.Order
{
    public class OrderItems : BaseEntity<int>
    {
        public OrderItems() { }
        public OrderItems(ProductItemOrderd productItemOrderd,int price,int quantity) 
        { 
            this.productItemOrderd = productItemOrderd;
            this.Price = price;
            this.Quantity = quantity;
        }
        public int OrderItemsId { get; set; }
        public ProductItemOrderd productItemOrderd { get; set; }
        public int Price { get; set; }
        public int? Quantity { get; set; }

    }
}