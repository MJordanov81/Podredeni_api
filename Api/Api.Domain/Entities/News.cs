namespace Api.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class News
    {
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 3)]
        public string Content { get; set; }
    }
}
