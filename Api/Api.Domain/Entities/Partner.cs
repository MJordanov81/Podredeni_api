namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Partner
    {
        public string Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string WebUrl { get; set; }

        [Required]
        [StringLength(4000)]
        public string Details { get; set; }
    }
}
