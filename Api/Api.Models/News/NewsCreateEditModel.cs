namespace Api.Models.News
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Constants;

    public class NewsCreateEditModel
    {
        [Required]
        [StringLength(ModelConstants.NewsTitleMaxLength, 
            MinimumLength = ModelConstants.NewsTitleMinLength, 
            ErrorMessage = ModelConstants.NewsTitleLengthError)]
        public string Title { get; set; }

        [Required]
        [StringLength(ModelConstants.NewsImageUrlMaxLength, 
            MinimumLength = ModelConstants.NewsImageUrlMinLength, 
            ErrorMessage = ModelConstants.NewsImageUrlLengthError)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(ModelConstants.NewsContentMaxLength, 
            MinimumLength = ModelConstants.NewsContentMinLength, 
            ErrorMessage = ModelConstants.NewsContentLengthError)]
        public string Content { get; set; }
    }
}
