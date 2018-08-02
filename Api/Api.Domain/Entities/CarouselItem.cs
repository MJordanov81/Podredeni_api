namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class CarouselItem
    {
        public string Id { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(150)]
        public string Heading { get; set; }

        [StringLength(500)]
        public string Content { get; set; }
    }
}
