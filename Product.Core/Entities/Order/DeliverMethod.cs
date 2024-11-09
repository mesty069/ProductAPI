namespace Product.Core.Entities.Order
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod() { }
        public DeliveryMethod(string shortName, string deliveryTime, string description, int price) 
        {
            ShortName = shortName;
            DeliveryTime = deliveryTime;
            Description = description;
            Price = price;
        }
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}