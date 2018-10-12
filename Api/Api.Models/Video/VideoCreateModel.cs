namespace Api.Models.Video
{
    using Api.Models.Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class VideoCreateModel
    {
        [Required]
        [StringLength(ModelConstants.VideoDescriptionLength,
                ErrorMessage = ModelConstants.VideoDescriptionLengthError)]
        public string Description { get; set; }

        [Required]
        [StringLength(ModelConstants.VideoUrlLength,
        ErrorMessage = ModelConstants.VideoUrlLengthError)]
        public string Url { get; set; }
    }
}
