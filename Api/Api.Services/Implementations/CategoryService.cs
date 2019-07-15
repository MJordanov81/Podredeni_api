namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Category;
    using Api.Models.Product;
    using Api.Models.Shared;
    using Api.Models.Subcategory;
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

        private readonly IProductService products;

        public CategoryService(ApiDbContext db, IProductService products)
        {
            this.db = db;
            this.products = products;
        }

        public async Task<ICollection<CategoryDetailsModel>> GetAll()
        {
            return this.db.Categories.ProjectTo<CategoryDetailsModel>().OrderBy(c => c.Place).ToList();
        }

        public async Task<string> Create(string categoryName)
        {
            if (await this.db.Categories.AnyAsync(c => c.Name == categoryName)) throw new ArgumentException(ErrorMessages.InvalidCategoryName);

            Category category = new Category
            {
                Name = categoryName
            };

            await this.SetInitialPlace(category, true);

            await this.db.Categories.AddAsync(category);

            await this.db.SaveChangesAsync();

            return category.Id;
        }

        public async Task<CategoriesDetailsListPaginatedModel> Get(PaginationModel pagination)
        {
            IEnumerable<CategoryDetailsModel> categories = await this.db.Categories
                .ProjectTo<CategoryDetailsModel>()
                .OrderBy(c => c.Place)
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

        public async Task UpdatePlace(string categoryId, int newPlace)
        {
            Category category = await this.db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            int currentPlace = category.Place;

            await this.IncrementPlacesStartingAt(currentPlace);

            category.Place = newPlace;

            await this.db.SaveChangesAsync();
        }

        public async Task UpdateName(string categoryId, string categoryName)
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

        public async Task<ICollection<NestedCategoryWithProductsDetailsModel>> GetAllNested(int numberOfProductsPerCategory = 3)
        {
            ICollection<NestedCategoryWithProductsDetailsModel> result = this.db.Categories
                .ProjectTo<NestedCategoryWithProductsDetailsModel>()
                .OrderBy(c => c.Place)
                .ToList();

            ICollection<Product> products = this.db.Products.Include(p => p.CategoryProducts).Include(p => p.SubcategoryProducts).ToList();

            ICollection<Subcategory> subcategories = this.db.Subcategories.ToList();

            foreach (Product product in products)
            {
                ProductDetailsModel productModel = await this.products.Get(product.Id);

                NestedCategoryWithProductsDetailsModel category = result
                    .Where(ncdm => ncdm.Id == product.CategoryProducts.FirstOrDefault()
                    .CategoryId)
                    .FirstOrDefault();

                if(category == null)
                {
                    continue;
                }

                category.Count++;

                category.Products.Add(productModel);

                if(product.SubcategoryProducts.Any())
                {
                    string scId = product.SubcategoryProducts.FirstOrDefault().SubcategoryId;

                    if (!category.Subcategories.Any(c => c.Id == scId))
                    {
                        Subcategory sc = subcategories.FirstOrDefault(c => c.Id == scId);

                        category.Subcategories.Add(new Models.Subcategory.NestedSubcategoryDetailsModel { Id = sc.Id, Name = sc.Name, Count = 0 });
                    }

                    category.Subcategories.FirstOrDefault(sc => sc.Id == scId).Count++;
                }
            }

            foreach (var category in result)
            {
                await this.AddProducts(category, numberOfProductsPerCategory);
            }

            return result;
        }

        private  async Task AddProducts(NestedCategoryWithProductsDetailsModel category, int numberOfProductsPerCategory)
        {
            numberOfProductsPerCategory = 3;

            Dictionary<string, int> productPlaces = this.db.CategoryProducts
                .Where(cp => cp.CategoryId == category.Id)
                .Select(c => new { c.ProductId, c.Place})
                .ToDictionary(c => c.ProductId, c => c.Place);

            category.Products = category.Products
                .Select(cp => 
                    {
                        return new { product = cp, place = productPlaces[cp.Id] };
                    })
                .OrderBy(p => p.place).Take(numberOfProductsPerCategory)
                .Select(p => p.product).ToList();
        }

        public async Task Reorder(ICollection<string> categories, ICollection<int> places)
        {
            IDictionary<string, int> categoriesAndPlaces = categories.Zip(places, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);

            foreach (var element in categoriesAndPlaces)
            {
                this.db.Categories.FirstOrDefault(c => c.Id == element.Key).Place = element.Value;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task ReorderProducts(string categoryId, ICollection<string> products)
        {
            var categoryProducts = this.db.CategoryProducts.Where(cp => cp.CategoryId == categoryId).ToList();

            List<string> productIds = products as List<string>;

            for (int i = 0; i < categoryProducts.Count; i++)
            {
                categoryProducts[i].Place = productIds.IndexOf(categoryProducts[i].ProductId) + 1;
            }

            await this.db.SaveChangesAsync();
        }

        public void CheckCategoryOrderAndInsertPlaces()
        {
            var categories = this.db.Categories.ToList();

            if(categories.GroupBy(c => c.Place).Any(g => g.Count() > 1))
            {
                var place = 1;

                foreach (var category in categories.Where(c => c.Name != "Default"))
                {
                    category.Place = place++;
                }

                this.db.SaveChanges();
            }
        }

        private async Task SetInitialPlace(Category category, bool setLast)
        {
            if (setLast)
            {
                category.Place = this.db.Categories.Count() + 1;
            }

            await this.IncrementPlacesStartingAt(1);

            category.Place = 1;
        }

        private async Task IncrementPlacesStartingAt(int fromPlace)
        {
            await this.db.Categories.Where(c => c.Place >= fromPlace).ForEachAsync(c => c.Place++);

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
