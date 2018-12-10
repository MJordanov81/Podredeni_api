namespace Api.Models.Order
{
    using Api.Models.ProductOrder;
    using Infrastructure.Constants;
    using Infrastructure.Filters;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderWithoutUserCreateModel
    {
        [CollectionNotEmpty(ErrorMessage = ModelConstants.ProductsEmptyArrayError)]
        public ICollection<ProductOrderCreateModel> Products { get; set; } = new List<ProductOrderCreateModel>();

        public string InvoiceDataId { get; set; }

        [Required]
        public string DeliveryDataId { get; set; }

        public string PromoCode { get; set; }

        public int PromotionsCount { get; set; }
    }
}
