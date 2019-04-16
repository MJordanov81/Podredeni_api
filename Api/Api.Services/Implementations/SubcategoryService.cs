namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Category;
    using Api.Models.Shared;
    using AutoMapper.QueryableExtensions;
    using Infrastructure.Constants;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SubcategoryService : ISubcategoryService
    {
        private readonly ApiDbContext db;

        public SubcategoryService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<CategoryDetailsModel>> GetAll()
        {
            return this.db.Subcategories.ProjectTo<CategoryDetailsModel>().ToList();
        }

        public async Task<string> Create(string categoryName)
        {
            if (await this.db.Subcategories.AnyAsync(c => c.Name == categoryName)) throw new ArgumentException(ErrorMessages.InvalidSubcategoryName);

            Subcategory subcategory = new Subcategory
            {
                Name = categoryName
            };

            await this.db.Subcategories.AddAsync(subcategory);

            await this.db.SaveChangesAsync();

            return subcategory.Id;
        }

        public async Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination)
        {
            IEnumerable<CategoryDetailsModel> subcategories = await this.db.Subcategories
                .ProjectTo<CategoryDetailsModel>()
                .ToListAsync();

            if (!string.IsNullOrEmpty(pagination.FilterElement))
            {
                subcategories = await this.FilterElements(subcategories, pagination.FilterElement, pagination.FilterValue);
            }

            if (!string.IsNullOrEmpty(pagination.SortElement))
            {
                subcategories = await this.SortElements(subcategories, pagination.SortElement, pagination.SortDesc);
            }

            int subcategoriesCount = subcategories.Count();

            if (pagination.Page < 1) pagination.Page = 1;

            if (pagination.Size <= 1) pagination.Size = subcategoriesCount;

            subcategories = subcategories.Skip(pagination.Size * (pagination.Page - 1)).Take(pagination.Size).ToList();

            return new CategoriesDetailsListPaginatedModel
            {
                Categories = subcategories,
                CategoriesCount = subcategoriesCount
            };

        }

        public async Task Update(string categoryId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentException(ErrorMessages.InvalidCategoryName);
            }

            if (await this.db.Subcategories.AnyAsync(c => c.Name == categoryName)) throw new ArgumentException(ErrorMessages.InvalidCategoryName);

            if (!await this.db.Subcategories.AnyAsync(c => c.Id == categoryId))
            {
                throw new ArgumentException(ErrorMessages.InvalidCategoryId);
            }

            Subcategory subcategory = await this.db.Subcategories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            subcategory.Name = categoryName;

            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string subcategoryId)
        {
            if (!await this.db.Subcategories.AnyAsync(c => c.Id == subcategoryId))
            {
                throw new InvalidOperationException(ErrorMessages.InvalidCategoryId);
            }

            if (await this.db.SubcategoryProducts.AnyAsync(sc => sc.SubcategoryId == subcategoryId))
            {
                throw new InvalidOperationException(ErrorMessages.InvalidCategoryDelete);
            }

            Subcategory subcategory = await this.db.Subcategories
                .FirstOrDefaultAsync(c => c.Id == subcategoryId);

            this.db.Subcategories.Remove(subcategory);

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

        public Task<string> SeedDefaultCategory()
        {
            throw new NotImplementedException();
        }
    }
}
