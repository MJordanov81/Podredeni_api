namespace Api.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Video
    {
        public string Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Url { get; set; }

        [Range(1, int.MaxValue)]
        public int Index { get; set; }
    }
}
