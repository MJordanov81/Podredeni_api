namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class PartnerLocation
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(300)]
        public string Address { get; set; }

        public Partner Partner { get; set; }

        public string PartnerId { get; set; }
    }
}
