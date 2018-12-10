namespace Api.Models.Promotion
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using System;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class PromotionDetailsModel : IMapFrom<Promotion>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PromoCode { get; set; }

        public bool IsInclusive { get; set; }

        public bool IsAccumulative { get; set; }

        public int ProductsCount { get; set; }

        public int DiscountedProductsCount { get; set; }

        public decimal Discount { get; set; }

        public string Description { get; set; }

        public bool IncludePriceDiscounts { get; set; }

        public int Quota { get; set; }

        public int UsedQuota { get; set; }

        public ICollection<string> DiscountedProductsIds { get; set; } = new List<string>();

        public ICollection<string> ProductsIds { get; set; } = new List<string>();

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Promotion, PromotionDetailsModel>()
                .ForMember(pdm => pdm.DiscountedProductsIds,                
                cfg => cfg.MapFrom(p => p.DiscountedProductsPromotions.Select(pp => pp.ProductId)))
                .ForMember(pdm => pdm.ProductsIds,                
                cfg => cfg.MapFrom(p => p.ProductsPromotions.Select(pp => pp.ProductId)));
        }
    }
}
