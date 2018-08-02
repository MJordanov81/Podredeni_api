namespace Api.Services.Interfaces
{
    using Api.Models.CarouselItem;
    using Api.Models.HomeContent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHomeContentService
    {
        Task ModifyArticle(HomeContentCreateEditModel content);

        Task<HomeContentModel> GetArticle();

        Task<string> CreateCarouselItem(CarouselItemCreateEditModel data);

        Task<string> EditCarouselItem(string id, CarouselItemCreateEditModel data);

        Task DeleteCarouselItem(string id);

        Task<CarouselItemDetailsModel> GetCarouselItem(string id);

        Task<IEnumerable<CarouselItemDetailsModel>> GetAllCarouselItems();
    }
}
