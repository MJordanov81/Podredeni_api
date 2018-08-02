namespace Api.Services.Interfaces
{
    using Api.Models.News;
    using Api.Models.Shared;
    using System.Threading.Tasks;

    public interface INewsService
    {
        Task<string> Create(NewsCreateEditModel data);

        Task<NewsListPaginatedModel> GetList(SimplePaginationModel pagination);

        Task<NewsDetailsModel> Get(string newsId);

        Task<string> Edit(string newsId, NewsCreateEditModel news);

        Task Delete(string newsId);
    }
}
