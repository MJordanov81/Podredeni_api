namespace Api.Models.Category
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class CategoryDetailsModel : IMapFrom<Category>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
