namespace Api.Models.PromoDiscount
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PromoDiscountCreateModel
    {
        [Required]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public decimal Discount { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters long")]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
