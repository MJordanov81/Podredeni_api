namespace Api.Domain.Entities
{
    public class SubcategoryProduct
    {
        public string SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
