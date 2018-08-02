namespace Api.Models.News
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using System;

    public class NewsListModel : IMapFrom<News>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
