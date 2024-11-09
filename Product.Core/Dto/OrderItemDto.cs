namespace Product.Core.Dto
{
    public class OrderItemDto
    {
        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string PrictureUrl { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
}