using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Entities
{
    public class Category
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        [Range(1, int.MaxValue)]
        public int Place { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
    }
}
