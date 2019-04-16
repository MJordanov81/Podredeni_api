namespace Api.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public string Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsTopSeller { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

        public ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();

        public ICollection<SubcategoryProduct> SubcategoryProducts { get; set; } = new List<SubcategoryProduct>();

        public ICollection<ProductPromoDiscount> ProductPromoDiscounts { get; set; } = new List<ProductPromoDiscount>();

        public ICollection<ProductPromotion> ProductsPromotions { get; set; } = new List<ProductPromotion>();

        public ICollection<DiscountedProductPromotion> DiscountedProductsPromotions { get; set; } = new List<DiscountedProductPromotion>();
    }
}
