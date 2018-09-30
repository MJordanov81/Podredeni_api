namespace Api.Services.Implementations
{
    using System.Threading.Tasks;
    using Api.Data;
    using Api.Services.Interfaces;
    using Api.Domain.Entities;
    using System;
    using Api.Services.Infrastructure.Constants;
    using System.Linq;
    using Api.Models.Video;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class VideoService : IVideoService
    {
        private readonly ApiDbContext db;

        public VideoService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                Video video = new Video
                {
                    Url = url
                };

                await this.db.Videos.AddAsync(video);

                await this.db.SaveChangesAsync();

                return video.Id;
            }

            else
            {
                throw new ArgumentException(ErrorMessages.InvalidVideoUrl);
            }
        }

        public async Task Delete(string videoId)
        {
            if (!this.db.Videos.Any(v => v.Id == videoId)) throw new ArgumentException(ErrorMessages.InvalidVideoId);

            Video video = this.db.Videos.FirstOrDefault(v => v.Id == videoId);

            this.db.Remove(video);

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<VideoDetailsModel>> GetAll()
        {
            return await this.db.Videos
                .ProjectTo<VideoDetailsModel>()
                .ToListAsync();
        }
    }
}
