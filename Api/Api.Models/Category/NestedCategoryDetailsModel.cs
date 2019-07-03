namespace Api.Models.Category
{
    using Api.Common.Mapping;
    using Api.Models.Subcategory;
    using System.Collections.Generic;
    using Api.Domain.Entities;
    using AutoMapper;

    public class NestedCategoryDetailsModel : IMapFrom<Category>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public ICollection<NestedSubcategoryDetailsModel> Subcategories { get; set; } = new List<NestedSubcategoryDetailsModel>();

        public void ConfigureMapping(Profile mapper)
        {
            mapper
                .CreateMap<Category, NestedCategoryDetailsModel>()
                .ForMember(x => x.Subcategories, opt => opt.Ignore())
                .ForMember(x => x.Count, opt => opt.Ignore());
        }
    }
}
