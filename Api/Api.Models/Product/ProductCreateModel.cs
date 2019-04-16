namespace Api.Models.Product
{
    using Api.Models.Infrastructure.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductCreateModel
    {
        [Required]
        [StringLength(ModelConstants.NameLength, 
            ErrorMessage = ModelConstants.NameLengthError)]
        public string Name { get; set; }

        [Required]
        [StringLength(ModelConstants.DescriptionLength, 
            ErrorMessage = ModelConstants.DescriptionLengthError)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, 
            ErrorMessage = ModelConstants.PriceRangeError)]
        public decimal Price { get; set; }

        public ICollection<string> Categories { get; set; }

        public ICollection<string> Subcategories { get; set; }

        public bool IsTopSeller { get; set; }

        public IList<string> ImageUrls { get; set; }
    }
}
