namespace Api.Models.Cart
{
    public class ProductInCartModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }
    }
}

