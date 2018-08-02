namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.News;
    using Api.Models.Shared;
    using AutoMapper.QueryableExtensions;
    using Infrastructure.Constants;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class NewsService : INewsService
    {
        private readonly ApiDbContext db;

        public NewsService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(NewsCreateEditModel data)
        {
            if(string.IsNullOrWhiteSpace(data.Title) || string.IsNullOrWhiteSpace(data.Content) || string.IsNullOrWhiteSpace(data.ImageUrl)) throw new ArgumentException(ErrorMessages.InvalidNewsParameters);

            DateTime creationDate = this.GetSofiaLocalTime();

            News news = new News
            {
                Title = data.Title,
                Content = data.Content,
                ImageUrl = data.ImageUrl,
                CreationDate = creationDate
            };

            try
            {
                await this.db.News.AddAsync(news);

                await this.db.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException(ErrorMessages.UnableToWriteToDb);
            }

            return news.Id;
        }

        public async Task Delete(string newsId)
        {
            News news = await this.db.News
                .Where(n => n.Id == newsId)
                .FirstOrDefaultAsync();

            if (news == null) throw new ArgumentException(ErrorMessages.InvalidNewsId);

            this.db.News.Remove(news);

            await this.db.SaveChangesAsync();
        }

        public async Task<string> Edit(string newsId, NewsCreateEditModel newsModified)
        {
            if (string.IsNullOrWhiteSpace(newsModified.Content) || 
                string.IsNullOrWhiteSpace(newsModified.ImageUrl) || 
                string.IsNullOrWhiteSpace(newsModified.Title))
                throw new ArgumentException(ErrorMessages.InvalidNewsParameters);

            if (!this.db.News.Any(n => n.Id == newsId))
                throw new ArgumentException(ErrorMessages.InvalidNewsId);

            News news = await this.db.News
                .Where(n => n.Id == newsId)
                .FirstOrDefaultAsync();

            news.Title = newsModified.Title;
            news.Content = newsModified.Content;
            news.ImageUrl = newsModified.ImageUrl;

            await this.db.SaveChangesAsync();

            return news.Id;
        }

        public async Task<NewsDetailsModel> Get(string newsId)
        {
            if (!this.db.News.Any(n => n.Id == newsId)) throw new ArgumentException(ErrorMessages.InvalidNewsId);

            return this.db.News
                .Where(n => n.Id == newsId)
                .ProjectTo<NewsDetailsModel>()
                .FirstOrDefault();
        }

        public async Task<NewsListPaginatedModel> GetList(SimplePaginationModel pagination)
        {
            IEnumerable<NewsListModel> news = this.db.News
                .OrderByDescending(n => n.CreationDate)
                .ProjectTo<NewsListModel>()
                .ToList();

            int newsCount = news.Count();

            news = news.Skip(pagination.Size * (pagination.Page - 1)).Take(pagination.Size).ToList();

            return new NewsListPaginatedModel
            {
                News = news,
                NewsCount = newsCount
            };
        }

        private DateTime GetSofiaLocalTime()
        {
            string ZoneId = "FLE Standard Time";
            DateTime serverLocalTime = DateTime.Now;
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(ZoneId);
            return TimeZoneInfo.ConvertTime(serverLocalTime, TimeZoneInfo.Local, timeZoneInfo);
        }
    }
}
