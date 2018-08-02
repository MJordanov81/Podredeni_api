namespace Api.Models.News
{
    using System.Collections.Generic;

    public class NewsListPaginatedModel
    {
        public IEnumerable<NewsListModel> News { get; set; }

        public int NewsCount { get; set; }
    }
}
