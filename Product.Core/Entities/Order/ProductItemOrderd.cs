namespace Product.Core.Entities.Order
{
    public class ProductItemOrderd
    {
        public ProductItemOrderd() { }
        public ProductItemOrderd(int productItemId, string productItemName, string prictureUrl)
        {
            ProductItemId = productItemId;
            ProductItemName = productItemName;
            PrictureUrl = prictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductItemName { get; set; }
        public string PrictureUrl { get; set; }
    }
}