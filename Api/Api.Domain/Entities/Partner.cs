namespace Api.Domain.Entities
{
    using System.Collections.Generic;
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
        [StringLength(20)]
        public string Category { get; set; }

        [Range(1, int.MaxValue)]
        public int Index { get; set; }

        public ICollection<PartnerLocation> PartnerLocations { get; set; } = new List<PartnerLocation>();
    }
}
