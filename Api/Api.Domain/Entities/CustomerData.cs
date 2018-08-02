namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class CustomerData
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerLastName { get; set; }

        [Required]
        [StringLength(100)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
