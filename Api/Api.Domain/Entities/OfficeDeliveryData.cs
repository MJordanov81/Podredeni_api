namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class OfficeDeliveryData
    {
        public string Id { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public string Code { get; set; }
    }
}
