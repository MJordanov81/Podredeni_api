namespace Api.Models.Partner
{
    using Api.Models.Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class PartnerLocationCreateModel
    {
        [Required]
        [StringLength(ModelConstants.PartnerLocationCityMaxLength, ErrorMessage = ModelConstants.PartnerLocationCityLengthError)]
        public string City { get; set; }

        [Required]
        [StringLength(ModelConstants.PartnerLocationAddressMaxLength, ErrorMessage = ModelConstants.PartnerLocationAddressLengthError)]
        public string Address { get; set; }
    }
}
