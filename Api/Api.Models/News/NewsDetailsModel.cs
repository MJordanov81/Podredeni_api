namespace Api.Models.News
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class NewsDetailsModel : NewsListModel, IMapFrom<News>
    {
        public string Content { get; set; }
    }
}
