namespace Api.Models.PromoDiscount
{
    using Api.Models.Infrastructure.Constants;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PromoDiscountCreateModel
    {
        [Required]
        [Range(ModelConstants.PromoDiscountDiscountMinValue, ModelConstants.PromoDiscountDiscountMaxValue, ErrorMessage = ModelConstants.PromoDiscountDiscountError)]
        public decimal Discount { get; set; }

        [Required]
        [StringLength(ModelConstants.PromoDiscountNameMaxLenght, MinimumLength = ModelConstants.PromoDiscountNameMinLenght, ErrorMessage = ModelConstants.PromoDiscountNameError)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
