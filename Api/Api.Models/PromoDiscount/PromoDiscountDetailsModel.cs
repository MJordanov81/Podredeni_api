namespace Api.Models.PromoDiscount
{
    using Api.Common.Mapping;
    using System;
    using Api.Domain.Entities;

    public class PromoDiscountDetailsModel : IMapFrom<PromoDiscount>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Discount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
