namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class DeliveryData
    {
        public string Id { get; set; }

        [StringLength(2500)]
        public string Comments { get; set; }

        public bool DeliveredToAnOffice { get; set; }

        [Required]
        public string CustomerDataId { get; set; }

        public CustomerData CustomerData { get; set; }

        public string HomeDeliveryDataId { get; set; }

        public HomeDeliveryData HomeDeliveryData { get; set; }

        public string OfficeDeliveryDataId { get; set; }

        public OfficeDeliveryData OfficeDeliveryData { get; set; }
    }
}
