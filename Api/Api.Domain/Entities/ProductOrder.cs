namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ProductOrder
    {
        [Required]
        public string ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
