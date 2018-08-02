using System.Collections.Generic;

namespace Api.Domain.Entities
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
    }
}
