namespace Api.Models.Category
{
    using System.Collections.Generic;

    public class ProductsReorderModel
    {
        public ICollection<string> Products { get; set; }
    }
}
