namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.CarouselItem;
    using Api.Models.HomeContent;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeContentService : IHomeContentService
    {
        private readonly ApiDbContext db;

        public HomeContentService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<HomeContentModel> GetArticle()
        {
            if (!await this.db.HomeContent.AnyAsync()) throw new InvalidOperationException("Content does not exist.");

            return await this.db.HomeContent
                .ProjectTo<HomeContentModel>()
                .FirstOrDefaultAsync();
        }

        public async Task ModifyArticle(HomeContentCreateEditModel content)
        {
            if (string.IsNullOrWhiteSpace(content.SectionHeading) ||
                string.IsNullOrWhiteSpace(content.SectionContent) ||
                string.IsNullOrWhiteSpace(content.ArticleHeading) ||
                string.IsNullOrWhiteSpace(content.ArticleContent))
                throw new ArgumentException("None of home content data could be an empty string");



            HomeContent persistedContent = await this.db.HomeContent.FirstOrDefaultAsync();

            if (persistedContent != null)
            {
                persistedContent.SectionHeading = content.SectionHeading;
                persistedContent.SectionContent = content.SectionContent;
                persistedContent.ArticleHeading = content.ArticleHeading;
                persistedContent.ArticleContent = content.ArticleContent;
            }

            else
            {
                HomeContent homeContent = new HomeContent
                {
                    ArticleHeading = content.ArticleHeading,
                    ArticleContent = content.ArticleContent,
                    SectionHeading = content.SectionHeading,
                    SectionContent = content.SectionContent
                };

                await this.db.HomeContent.AddAsync(homeContent);
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<string> CreateCarouselItem(CarouselItemCreateEditModel data)
        {

            if (string.IsNullOrEmpty(data.ImageUrl)) throw new ArgumentException("ImageUrl cannot be an empty string");

            CarouselItem carouselItem = new CarouselItem
            {
                ImageUrl = data.ImageUrl,
                Heading = data.Heading,
                Content = data.Content
            };

            await this.db.CarouselItems.AddAsync(carouselItem);

            await this.db.SaveChangesAsync();

            return carouselItem.Id;
        }

        public async Task DeleteCarouselItem(string id)
        {
            if (!await this.db.CarouselItems
                .AnyAsync(i => i.Id == id)) throw new ArgumentException("Item with the given id does not exist.");

            CarouselItem item = await this.db.CarouselItems
                .FirstOrDefaultAsync(i => i.Id == id);

            this.db.CarouselItems.Remove(item);

            await this.db.SaveChangesAsync();
        }

        public async Task<string> EditCarouselItem(string id, CarouselItemCreateEditModel data)
        {
            if (string.IsNullOrEmpty(data.ImageUrl)) throw new ArgumentException("ImageUrl cannot be an empty string");

            if (!await this.db.CarouselItems
                .AnyAsync(i => i.Id == id)) throw new ArgumentException("Item with the given id does not exist.");

            CarouselItem item = await this.db.CarouselItems
                .FirstOrDefaultAsync(i => i.Id == id);

            item.ImageUrl = data.ImageUrl;
            item.Heading = data.Heading;
            item.Content = data.Content;

            await this.db.SaveChangesAsync();

            return item.Id;
        }

        public async Task<IEnumerable<CarouselItemDetailsModel>> GetAllCarouselItems()
        {
            return await this.db.CarouselItems
                .ProjectTo<CarouselItemDetailsModel>()
                .ToListAsync();
        }

        public async Task<CarouselItemDetailsModel> GetCarouselItem(string id)
        {
            if (!await this.db.CarouselItems
                .AnyAsync(i => i.Id == id)) throw new ArgumentException("Item with the given id does not exist.");

            return await this.db.CarouselItems
                .Where(i => i.Id == id)
                .ProjectTo<CarouselItemDetailsModel>()
                .FirstOrDefaultAsync();

        }
    }
}
