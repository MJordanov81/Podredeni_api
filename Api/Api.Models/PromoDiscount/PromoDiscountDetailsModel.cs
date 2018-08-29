namespace Api.Models.PromoDiscount
{
    using Api.Common.Mapping;
    using System;
    using Api.Domain.Entities;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class PromoDiscountDetailsModel : IMapFrom<PromoDiscount>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Discount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<string> ProductsIds { get; set; }
    }
}
