namespace Api.Models.DeliveryData
{
    public class DeliveryDataDetailsModel
    {
        //Customer data
        public string CustomerName { get; set; }

        public string CustomerLastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        //Home delivery data

        public string Country { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string Street { get; set; }

        public string StreetNumber { get; set; }

        public string District { get; set; }

        public string Block { get; set; }

        public string Entrance { get; set; }

        public string Floor { get; set; }

        public string Apartment { get; set; }

        public string Comments { get; set; }

        public bool DeliveredToAnOffice { get; set; }

        //Office delivery data
        public string OfficeAddress { get; set; }

        public string OfficeCity { get; set; }

        public string OfficeCountry { get; set; }

        public string OfficeName { get; set; }

        public string OfficeCode { get; set; }
    }
}
