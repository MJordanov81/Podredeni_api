namespace Api.Models.ProductOrder
{
    public class ProductOrderCreateModel
    {
        public string Id { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }
    }
}
