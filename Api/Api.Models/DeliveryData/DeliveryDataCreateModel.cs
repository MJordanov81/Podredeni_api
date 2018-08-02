namespace Api.Models.DeliveryData
{
    using Api.Models.Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class DeliveryDataCreateModel
    {
        [Required]
        [StringLength(ModelConstants.CustomerNameLength, ErrorMessage = ModelConstants.CustomerNameLengthError)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(ModelConstants.CustomerNameLength, ErrorMessage = ModelConstants.CustomerNameLengthError)]
        public string CustomerLastName { get; set; }

        [Required]
        [StringLength(ModelConstants.PhoneNumberLength, ErrorMessage = ModelConstants.PhoneNumberLengthError)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(ModelConstants.EmailLength, ErrorMessage = ModelConstants.EmailLengthError)]
        public string Email { get; set; }



        [StringLength(ModelConstants.CountryLength, ErrorMessage = ModelConstants.CountryLengthError)]
        public string Country { get; set; }

        [StringLength(ModelConstants.CityLength, ErrorMessage = ModelConstants.CityLengthError)]
        public string City { get; set; }

        [StringLength(ModelConstants.PostCodeLength, ErrorMessage = ModelConstants.PostCodeLengthError)]
        public string PostCode { get; set; }

        [StringLength(ModelConstants.StreetLength, ErrorMessage = ModelConstants.StreetLengthError)]
        public string Street { get; set; }

        [StringLength(ModelConstants.StreetNumberLength, ErrorMessage = ModelConstants.StreetNumberLengthError)]
        public string StreetNumber { get; set; }

        [StringLength(ModelConstants.DistrictLength, ErrorMessage = ModelConstants.DistrictLengthError)]
        public string District { get; set; }

        [StringLength(ModelConstants.BlockLength, ErrorMessage = ModelConstants.BlockLengthError)]
        public string Block { get; set; }

        [StringLength(ModelConstants.EntranceLength, ErrorMessage = ModelConstants.EntranceLengthError)]
        public string Entrance { get; set; }

        [StringLength(ModelConstants.FloorLength, ErrorMessage = ModelConstants.FloorLengthError)]
        public string Floor { get; set; }

        [StringLength(ModelConstants.ApartmentLength, ErrorMessage = ModelConstants.ApartmentLengthError)]
        public string Apartment { get; set; }

        [StringLength(ModelConstants.CommentsLength, ErrorMessage = ModelConstants.CommentsLengthError)]
        public string Comments { get; set; }




        public bool DeliveredToAnOffice { get; set; }




        [StringLength(ModelConstants.OfficeAddressLength, ErrorMessage = ModelConstants.OfficeAddressLengthError)]
        public string OfficeAddress { get; set; }

        [StringLength(ModelConstants.OfficeCityLength, ErrorMessage = ModelConstants.OfficeCityLengthError)]
        public string OfficeCity { get; set; }

        [StringLength(ModelConstants.OfficeCountryLength, ErrorMessage = ModelConstants.OfficeCountryLengthError)]
        public string OfficeCountry { get; set; }

        [StringLength(ModelConstants.OfficeNameLength, ErrorMessage = ModelConstants.OfficeNameLengthError)]
        public string OfficeName { get; set; }

        public string OfficeCode { get; set; }
    }
}
