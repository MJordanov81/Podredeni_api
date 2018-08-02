namespace Api.Models.CarouselItem
{
    using System.ComponentModel.DataAnnotations;

    public class CarouselItemCreateEditModel
    {
        [Required]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "ImageUrl must be between 3 and 500 characters long")]
        public string ImageUrl { get; set; }

        [StringLength(50, ErrorMessage = "Heading must be no more than 50 characters long")]
        public string Heading { get; set; }

        [StringLength(500, ErrorMessage = "Content must be no more than 500 characters long")]
        public string Content { get; set; }
    }
}
