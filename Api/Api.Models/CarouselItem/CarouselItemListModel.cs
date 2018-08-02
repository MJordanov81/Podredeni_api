namespace Api.Models.CarouselItem
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class CarouselItemListModel : IMapFrom<CarouselItem>
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string Heading { get; set; }
    }
}
