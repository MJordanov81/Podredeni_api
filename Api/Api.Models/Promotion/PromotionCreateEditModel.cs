namespace Api.Models.Promotion
{
    using Api.Models.Infrastructure.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PromotionCreateEditModel
    {
        [Required]
        [StringLength(ModelConstants.PromotionNameMaxLength, MinimumLength = ModelConstants.PromotionNameMinLength, ErrorMessage = ModelConstants.PromotionNameLengthError)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(ModelConstants.PromotionPromoCodeMaxLength, MinimumLength = ModelConstants.PromotionPromoCodeMinLength, ErrorMessage = ModelConstants.PromotionPromoCodeLengthError)]
        public string PromoCode { get; set; }

        public bool IsInclusive { get; set; }

        public bool IsAccumulative { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ModelConstants.PromotionProductsCountError)]
        public int ProductsCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ModelConstants.PromotionDiscountedProductsCountError)]
        public int DiscountedProductsCount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = ModelConstants.PromotionDiscountError)]
        public decimal Discount { get; set; }

        [StringLength(ModelConstants.PromotionDescriptionMaxLength, ErrorMessage = ModelConstants.PromotionDescriptionError)]
        public string Description { get; set; }

        public bool IncludePriceDiscounts { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ModelConstants.PromotionQuotaError)]
        public int Quota { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ModelConstants.PromotionQuotaError)]
        public int UsedQuota { get; set; }

        public ICollection<string> DiscountedProducts { get; set; }

        public ICollection<string> Products { get; set; }
    }
}
