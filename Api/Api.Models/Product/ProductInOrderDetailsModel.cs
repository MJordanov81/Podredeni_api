namespace Api.Models.Product
{
    public class ProductInOrderDetailsModel
    {
        public string Id { get; set; }

        public string ProductOrderId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }
    }
}
