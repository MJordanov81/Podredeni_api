namespace Api.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Image
    {
        public string Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Url { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
