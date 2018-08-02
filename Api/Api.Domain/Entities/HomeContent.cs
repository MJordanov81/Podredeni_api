namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class HomeContent
    {
        public string Id { get; set; }

        [Required]
        [StringLength(150)]
        public string SectionHeading { get; set; }

        [Required]
        [StringLength(1000)]
        public string SectionContent { get; set; }

        [Required]
        [StringLength(150)]
        public string ArticleHeading { get; set; }

        [Required]
        [StringLength(1000)]
        public string ArticleContent { get; set; }
    }
}
