namespace Api.Models.Subcategory
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class SubcategoryDetailsModel : IMapFrom<Subcategory>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
