namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class HomeDeliveryData
    {
        public string Id { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        [StringLength(10)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string District { get; set; }

        [StringLength(10)]
        public string Block { get; set; }

        [StringLength(10)]
        public string Entrance { get; set; }

        [StringLength(10)]
        public string Floor { get; set; }

        [StringLength(10)]
        public string Apartment { get; set; }
    }
}
