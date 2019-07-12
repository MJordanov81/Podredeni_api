namespace Api.Models.Category
{
    using Api.Models.Product;
    using AutoMapper;
    using System.Collections.Generic;
    using Api.Domain.Entities;

    public class NestedCategoryWithProductsDetailsModel : NestedCategoryDetailsModel
    {
        public ICollection<ProductDetailsModel> Products { get; set; }

        public override void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Category, NestedCategoryWithProductsDetailsModel>()
                .ForMember(x => x.Products, opt => opt.Ignore());
        }
    }
}
