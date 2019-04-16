namespace Api.Domain.Entities
{
    using System.Collections.Generic;

    public class Subcategory
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<SubcategoryProduct> SubcategoryProducts { get; set; } = new List<SubcategoryProduct>();
    }
}
