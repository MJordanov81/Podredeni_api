namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Video;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class VideoService : IVideoService
    {
        private readonly ApiDbContext db;

        public VideoService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(VideoCreateModel data)
        {
            if (!string.IsNullOrWhiteSpace(data.Description) && !string.IsNullOrWhiteSpace(data.Url))
            {
                await this.ReindexVideos(); //Increment each video's index with 1, so that the new video comes first

                Video video = new Video
                {
                    Url = data.Url,
                    Description = data.Description,
                    Index = 1
                    
                };

                await this.db.Videos.AddAsync(video);

                await this.db.SaveChangesAsync();

                return video.Id;
            }

            else
            {
                throw new ArgumentException(ErrorMessages.InvalidVideoCreateData);
            }
        }

        public async Task Delete(string videoId)
        {
            if (!this.db.Videos.Any(v => v.Id == videoId)) throw new ArgumentException(ErrorMessages.InvalidVideoId);

            Video video = this.db.Videos.FirstOrDefault(v => v.Id == videoId);

            this.db.Remove(video);

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<VideoDetailsModel>> Get()
        {
            return await this.db.Videos
                .OrderBy(v => v.Index)
                .ProjectTo<VideoDetailsModel>()
                .ToListAsync();
        }

        public async Task Reorder(string[] orderedVideoIds)
        {
            IEnumerable<Video> videos = await this.db.Videos.ToListAsync();

            foreach (Video video in videos)
            {
                int index = Array.IndexOf(orderedVideoIds, video.Id);

                video.Index = ++index;
            }

            await this.db.SaveChangesAsync();
        }

        private async Task ReindexVideos()
        {
            IEnumerable<Video> videos = await this.db.Videos.ToListAsync();

            foreach (Video video in videos)
            {
                video.Index++;
            }

            await this.db.SaveChangesAsync();
        }
    }
}
