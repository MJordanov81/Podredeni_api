namespace Api.Models.HomeContent
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class HomeContentModel : IMapFrom<HomeContent>
    {
        public string SectionHeading { get; set; }

        public string SectionContent { get; set; }

        public string ArticleHeading { get; set; }

        public string ArticleContent { get; set; }
    }
}
