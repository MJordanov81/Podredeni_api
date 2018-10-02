namespace Api.Models.Partner
{
    using Api.Models.Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class PartnerCreateEditModel
    {
        [Required]
        [StringLength(ModelConstants.PartnerNameMaxLength,
            ErrorMessage = ModelConstants.PartnerNameLengthError)]
        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public string WebUrl { get; set; }

        [Required]
        [StringLength(ModelConstants.PartnerDetailsMaxLength,
            ErrorMessage = ModelConstants.PartnerDetailsLengthError)]
        public string Details { get; set; }
    }
}
