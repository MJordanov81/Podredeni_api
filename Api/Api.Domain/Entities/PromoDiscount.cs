namespace Api.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PromoDiscount
    {
        public string Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }

        public ICollection<ProductPromoDiscount> ProductPromoDiscounts { get; set; } = new List<ProductPromoDiscount>();
    }
}
