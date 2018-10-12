namespace Api.Models.Video
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;

    public class VideoDetailsModel : IMapFrom<Video>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public int Index { get; set; }
    }
}
