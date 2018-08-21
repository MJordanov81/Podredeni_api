namespace Api.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    public class PromoDiscount
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Discount { get; set; }

        public ICollection<ProductPromoDiscount> ProductPromoDiscounts { get; set; } = new List<ProductPromoDiscount>();
    }
}
