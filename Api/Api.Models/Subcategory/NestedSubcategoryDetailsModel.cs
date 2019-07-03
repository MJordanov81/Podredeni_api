namespace Api.Models.Subcategory
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using AutoMapper;

    public class NestedSubcategoryDetailsModel : IMapFrom<Subcategory>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper
            .CreateMap<Category, NestedSubcategoryDetailsModel>()
            .ForMember(x => x.Count, opt => opt.Ignore());
        }
    }
}
