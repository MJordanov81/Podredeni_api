namespace Api.Services.Implementations
{
    using System.Threading.Tasks;
    using Api.Services.Interfaces;
    using Api.Data;
    using System.Linq;
    using System;
    using Api.Services.Infrastructure.Constants;
    using Microsoft.EntityFrameworkCore;
    using Api.Domain.Entities;

    public class CategoryService : ICategoryService
    {
        private readonly ApiDbContext db;

        public CategoryService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(string categoryName)
        {
            if (await this.db.Categories.AnyAsync(c => c.Name == categoryName)) throw new ArgumentException(ErrorMessages.InvalidCategoryName);

            Category category = new Category
            {
                Name = categoryName
            };

            await this.db.Categories.AddAsync(category);

            await this.db.SaveChangesAsync();

            return category.Id;

        }

        public async Task<string> SeedDefaultCategory()
        {
            if (!await this.db.Categories.AnyAsync())
            {
                Category defaultCategory = new Category
                {
                    Name = "Default"
                };

                await this.db.Categories.AddAsync(defaultCategory);

                await this.db.SaveChangesAsync();
            }

            return await this.db.Categories
                .Where(c => c.Name == "Default")
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
        }
    }
}
