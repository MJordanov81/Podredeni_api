namespace Api.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Promotion
    {
        public string Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(20, MinimumLength =6)]
        public string PromoCode { get; set; }

        public bool IsInclusive { get; set; }

        public bool IsAccumulative { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductsCount { get; set; }

        [Range(1, int.MaxValue)]
        public int DiscountedProductsCount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IncludePriceDiscounts { get; set; }

        [Range(0, int.MaxValue)]
        public int Quota { get; set; }

        [Range(0, int.MaxValue)]
        public int UsedQuota { get; set; }

        public ICollection<DiscountedProductPromotion> DiscountedProductsPromotions { get; set; } = new List<DiscountedProductPromotion>();

        public ICollection<ProductPromotion> ProductsPromotions { get; set; } = new List<ProductPromotion>();
    }
}
