namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Category;
    using Api.Models.Shared;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly ApiDbContext db;

        public CategoryService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<CategoryDetailsModel>> GetAll()
        {
            return this.db.Categories.ProjectTo<CategoryDetailsModel>().ToList();
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

        public async Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination)
        {
            IEnumerable<CategoryDetailsModel> categories = await this.db.Categories
                .ProjectTo<CategoryDetailsModel>()
                .ToListAsync();

            if (!string.IsNullOrEmpty(pagination.FilterElement))
            {
                categories = await this.FilterElements(categories, pagination.FilterElement, pagination.FilterValue);
            }

            if (!string.IsNullOrEmpty(pagination.SortElement))
            {
                categories = await this.SortElements(categories, pagination.SortElement, pagination.SortDesc);
            }

            int categoriesCount = categories.Count();

            if (pagination.Page < 1) pagination.Page = 1;

            if (pagination.Size <= 1) pagination.Size = categoriesCount;

            categories = categories.Skip(pagination.Size * (pagination.Page - 1)).Take(pagination.Size).ToList();

            return new CategoriesDetailsListPaginatedModel
            {
                Categories = categories,
                CategoriesCount = categoriesCount
            };

        }

        public async Task<string> SeedDefaultCategory()
        {
            if (!await this.db.Categories.AnyAsync(c => c.Name == "Default"))
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

        public async Task Update(string categoryId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException(ErrorMessages.InvalidCategoryName);
            }

            if (await this.db.Categories.AnyAsync(c => c.Name == categoryName)) throw new ArgumentException(ErrorMessages.InvalidCategoryName);

            if (!await this.db.Categories.AnyAsync(c => c.Id == categoryId))
            {
                throw new ArgumentException(ErrorMessages.InvalidCategoryId);
            }

            Category category = await this.db.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            category.Name = categoryName;

            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string categoryId)
        {
            if (!await this.db.Categories.AnyAsync(c => c.Id == categoryId))
            {
                throw new InvalidOperationException(ErrorMessages.InvalidCategoryId);
            }

            if (await this.db.CategoryProducts.AnyAsync(cp => cp.CategoryId == categoryId))
            {
                throw new InvalidOperationException(ErrorMessages.InvalidCategoryDelete);
            }

            Category category = await this.db.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            this.db.Categories.Remove(category);

            await this.db.SaveChangesAsync();
        }

        private async Task<IEnumerable<CategoryDetailsModel>> FilterElements(IEnumerable<CategoryDetailsModel> categories, string filterElement, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                filterElement = filterElement.ToLower();
                filterValue = filterValue.ToLower();

                if (filterElement == SortAndFilterConstants.Name)
                {
                    return categories.Where(p => p.Name.ToLower().Contains(filterValue));
                }
            }

            return categories;
        }

        private async Task<IEnumerable<CategoryDetailsModel>> SortElements(IEnumerable<CategoryDetailsModel> categories, string sortElement, bool sortDesc)
        {
            sortElement = sortElement.ToLower();

            if (sortElement == SortAndFilterConstants.Name)
            {
                if (sortDesc)
                {
                    return categories.OrderByDescending(p => p.Name).ToArray();
                }
                else
                {
                    return categories.OrderBy(p => p.Name).ToArray();
                }
            }
            return categories;
        }
    }
}
