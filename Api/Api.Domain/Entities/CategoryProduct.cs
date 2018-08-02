namespace Api.Domain.Entities
{
    public class CategoryProduct
    {
        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
