namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Video
    {
        public string Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Url { get; set; }
    }
}
