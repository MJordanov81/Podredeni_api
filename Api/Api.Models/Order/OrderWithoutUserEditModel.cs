namespace Api.Models.Order
{
    using Api.Domain.Enums;
    using Infrastructure.Constants;
    using Infrastructure.Filters;
    using ProductOrder;
    using System.Collections.Generic;

    public class OrderWithoutUserEditModel
    {
        [CollectionNotEmpty(ErrorMessage = ModelConstants.ProductsEmptyArrayError)]
        public ICollection<ProductOrderCreateModel> Products { get; set; } = new List<ProductOrderCreateModel>();
    }
}
