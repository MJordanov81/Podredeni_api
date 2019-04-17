namespace Api.Models.Product
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using Api.Models.Category;
    using Api.Models.Subcategory;
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductDetailsModel : IMapFrom<Product>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsTopSeller { get; set; }

        public ICollection<string> Images { get; set; }

        public decimal Discount { get; set; }

        public ICollection<string> PromoDiscountsIds { get; set; }

        public ICollection<CategoryDetailsModel> Categories { get; set; }

        public ICollection<SubcategoryDetailsModel> Subcategories { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Product, ProductDetailsModel>()
            .ForMember(m => m.Images, cfg => cfg.MapFrom(p => p.Images.OrderByDescending(i => i.CreationDateTime).Select(i => i.Url).ToList()));
        }
    }
}
