namespace Api.Services.Interfaces
{
    using Api.Models.Video;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IVideoService
    {
        Task<string> Create(VideoCreateModel data);

        Task Delete(string videoId);

        Task<IEnumerable<VideoDetailsModel>> Get();

        Task Reorder(string[] orderedVideoIds);
    }
}
