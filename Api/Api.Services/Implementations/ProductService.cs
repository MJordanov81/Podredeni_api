namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Category;
    using Api.Models.Product;
    using Api.Models.Shared;
    using Api.Models.Subcategory;
    using AutoMapper.QueryableExtensions;
    using Infrastructure.Constants;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly INumeratorService numerator;
        private readonly IImageService images;
        private readonly ICategoryService categories;
        private readonly ISubcategoryService subcategories;
        private readonly ApiDbContext db;

        public ProductService(INumeratorService numerator, IImageService images, ICategoryService categories, ISubcategoryService subcategories, ApiDbContext db)
        {
            this.numerator = numerator;
            this.images = images;
            this.categories = categories;
            this.subcategories = subcategories;
            this.db = db;
        }

        //Create a product
        public async Task<string> Create(ProductCreateModel data)
        {
            if (data.Name == null || data.Description == null || data.Price < 0) throw new ArgumentException(ErrorMessages.InvalidProductParameters);

            int number = await this.numerator.GetNextNumer(typeof(Product));

            Product product = new Product
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                IsTopSeller = data.IsTopSeller,
                Number = number
            };

            try
            {
                await this.db.Products.AddAsync(product);

                await this.db.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException(ErrorMessages.UnableToWriteToDb);
            }

            await CreateImages(data.ImageUrls, product.Id);

            await AddToCategories(data.Categories, product.Id);

            await AddToSubcategories(data.Subcategories, product.Id);

            return product.Id;
        }

        //Edit an existing product
        public async Task<string> Edit(string productId, ProductEditModel data)
        {
            if (data.Name == null || data.Description == null || data.Price < 0) throw new ArgumentException(ErrorMessages.InvalidProductParameters);

            if (!this.db.Products.Any(p => p.Id == productId)) throw new ArgumentException(ErrorMessages.InvalidProductId);

            Product product = await this.db.Products.FindAsync(productId);

            product.Name = data.Name;
            product.Description = data.Description;
            product.Price = data.Price;
            product.IsTopSeller = data.IsTopSeller;
            product.IsBlocked = data.IsBlocked;

            await this.db.SaveChangesAsync();

            await DeleteImages(productId);

            await CreateImages(data.ImageUrls, productId);

            await UpdateCategories(data.Categories, product.Id);

            await UpdateSubcategories(data.Subcategories, product.Id);

            return product.Id;
        }

        //Get details for a product
        public async Task<ProductDetailsModel> Get(string id)
        {
            if (!this.db.Products.Any(p => p.Id == id)) throw new ArgumentException(ErrorMessages.InvalidProductId);

            ProductDetailsModel product = this.db.Products
                .Where(p => p.Id == id)
                .ProjectTo<ProductDetailsModel>()
                .FirstOrDefault();

            product.PromoDiscountsIds = await this.GetAssociatedPromoDiscuntsIds(id);

            product.Discount = await this.CalculateDiscount(id);

            product.Categories = await this.GetAssociatedCategoryIds(product.Id);

            product.Subcategories = await this.GetAssociatedSubcategoryIds(product.Id);

            return product;
        }

        //Get details for a range of products - filtered, sorted and paginated
        public async Task<ProductDetailsListPaginatedModel> GetAll(PaginationModel pagination, ICollection<string> categories, ICollection<string> subcategories, bool includeBlocked)
        {

            ICollection<ProductDetailsModel> products = db.Products
                .ProjectTo<ProductDetailsModel>()
                .ToList();

            foreach (var product in products)
            {
                product.PromoDiscountsIds = await this.GetAssociatedPromoDiscuntsIds(product.Id);
            }

            foreach (var product in products)
            {
                product.Categories = await this.GetAssociatedCategoryIds(product.Id);
            }

            foreach (var product in products)
            {
                product.Subcategories = await this.GetAssociatedSubcategoryIds(product.Id);
            }

            foreach (ProductDetailsModel product in products)
            {
                product.Discount = await this.CalculateDiscount(product.Id);

            }

            if (!string.IsNullOrEmpty(pagination.FilterElement))
            {
                products = await this.FilterElements(products, pagination.FilterElement, pagination.FilterValue);
            }

            if (categories != null && categories.Count > 0)
            {
                products = await this.FilterCategories(products, categories);
            }

            if (subcategories != null && subcategories.Count > 0)
            {
                products = await this.FilterSubcategories(products, subcategories);
            }

            if (!string.IsNullOrEmpty(pagination.SortElement))
            {
                products = await this.SortElements(products, pagination.SortElement, pagination.SortDesc);
            }

            if (pagination.FilterElement.ToLower() != SortAndFilterConstants.IsBlocked && includeBlocked == false)
            {
                products = await this.FilterElements(products, SortAndFilterConstants.IsBlocked, false.ToString());
            }

            int productsCount = products.Count();

            if (pagination.Page < 1) pagination.Page = 1;

            if (pagination.Size <= 1) pagination.Size = productsCount;

            products = products.Skip(pagination.Size * (pagination.Page - 1)).Take(pagination.Size).ToList();

            ICollection<CategoryDetailsModel> uniqueCategories = await this.PopulateCategories(products);

            ICollection<SubcategoryDetailsModel> uniqueSubategories = await this.PopulateSubcategories(products);

            ProductDetailsListPaginatedModel result = new ProductDetailsListPaginatedModel
            {
                Products = products,
                ProductsCount = productsCount
            };

            result.Categories = uniqueCategories;
            result.Subcategories = uniqueSubategories;

            return result;
        }

        private async Task<ICollection<CategoryDetailsModel>> PopulateCategories(ICollection<ProductDetailsModel> products)
        {

            HashSet<CategoryDetailsModel> categories = new HashSet<CategoryDetailsModel>();

            foreach (ProductDetailsModel product in products)
            {
                product.Categories.ToList().ForEach(c => {
                    if(!categories.Select(cc => cc.Id).Contains(c.Id))
                    {
                        categories.Add(c);
                    }
                });
            }

            return categories;

        }

        private async Task<ICollection<SubcategoryDetailsModel>> PopulateSubcategories(ICollection<ProductDetailsModel> products)
        {

            HashSet<SubcategoryDetailsModel> subcategories = new HashSet<SubcategoryDetailsModel>();

            foreach (ProductDetailsModel product in products)
            {
                product.Subcategories.ToList().ForEach(c => {
                    if (!subcategories.Select(cc => cc.Id).Contains(c.Id))
                    {
                        subcategories.Add(c);
                    }
                });
            }

            return subcategories;

        }

        private async Task<ICollection<SubcategoryDetailsModel>> GetAssociatedSubcategoryIds(string productId)
        {
            ICollection<string> subcategoryIds = await this.db.SubcategoryProducts
                .Where(sc => sc.ProductId == productId)
                .Select(sc => sc.SubcategoryId)
                .ToListAsync();

            return await this.db.Subcategories.Where(c => subcategoryIds.Contains(c.Id)).ProjectTo<SubcategoryDetailsModel>().ToListAsync();
        }

        private async Task<ICollection<CategoryDetailsModel>> GetAssociatedCategoryIds(string productId)
        {
            ICollection<string> categoryIds = await this.db.CategoryProducts
                            .Where(sc => sc.ProductId == productId)
                            .Select(sc => sc.CategoryId)
                            .ToListAsync();

            return await this.db.Categories.Where(c => categoryIds.Contains(c.Id)).ProjectTo<CategoryDetailsModel>().ToListAsync();
        }


        private async Task<ICollection<string>> GetAssociatedPromoDiscuntsIds(string productId)
        {
            DateTime today = DateTime.Now.Date;

            return await this.db.ProductPromoDiscounts
                .Where(p => p.ProductId == productId && p.PromoDiscount.StartDate <= today && p.PromoDiscount.EndDate >= today)
                .Select(p => p.PromoDiscountId)
                .ToListAsync();
        }

        private async Task<decimal> CalculateDiscount(string productId)
        {
            decimal discount = 0;

            DateTime today = DateTime.Now.Date;

            if (this.db.ProductPromoDiscounts.Any(d => d.ProductId == productId))
            {
                discount = this.db.ProductPromoDiscounts
                    .Where(d => d.ProductId == productId)
                    .Select(d => d.PromoDiscount)
                    .Where(d => d.StartDate <= today && d.EndDate >= today)
                    .Sum(d => d.Discount);
            }

            //if (discount > 100) discount = 100;

            return discount;
        }

        #region "FilterAndSort"

        private async Task<ICollection<ProductDetailsModel>> FilterElements(ICollection<ProductDetailsModel> products, string filterElement, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                filterElement = filterElement.ToLower();
                filterValue = filterValue.ToLower();

                if (filterElement == SortAndFilterConstants.Name)
                {
                    return products.Where(p => p.Name.ToLower().Contains(filterValue)).ToList();
                }

                else if (filterElement == SortAndFilterConstants.Category)
                {
                    ICollection<ProductDetailsModel> result = new List<ProductDetailsModel>();

                    foreach (ProductDetailsModel product in products)
                    {
                        var categoriesNames = product.Categories.Select(c => c.Name.ToLower()).ToList();

                        foreach (string categoryName in categoriesNames)
                        {
                            if (categoryName.Contains(filterValue))
                            {
                                result.Add(product);
                            }
                        }
                    }

                    return result;
                }

                else if (filterElement == SortAndFilterConstants.Subcategory)
                {
                    ICollection<ProductDetailsModel> result = new List<ProductDetailsModel>();

                    foreach (ProductDetailsModel product in products)
                    {
                        var subcategoriesNames = product.Subcategories.Select(c => c.Name.ToLower()).ToList();

                        foreach (string subcategoryName in subcategoriesNames)
                        {
                            if (subcategoryName.Contains(filterValue))
                            {
                                result.Add(product);
                            }
                        }
                    }

                    return result;
                }

                else if (filterElement == SortAndFilterConstants.Number)
                {
                    return products.Where(p => p.Number.ToString().Contains(filterValue)).ToList();
                }

                else if (filterElement == SortAndFilterConstants.Quantity)
                {
                    bool isANumber = int.TryParse(filterValue, out int quantity);

                    if (isANumber) return products.Where(p => p.Quantity == quantity).ToList();
                }

                else if (filterElement == SortAndFilterConstants.IsBlocked)
                {
                    bool isBoolean = bool.TryParse(filterValue, out bool isBlocked);

                    if (isBoolean) return products.Where(p => p.IsBlocked == isBlocked).ToList();
                }


                else if (filterElement == SortAndFilterConstants.IsTopSeller)
                {
                    bool isBoolean = bool.TryParse(filterValue, out bool isTopSeller);

                    if (isBoolean) return products.Where(p => p.IsTopSeller == isTopSeller).ToList();
                }
            }

            return products;
        }

        private async Task<ICollection<ProductDetailsModel>> SortElements(ICollection<ProductDetailsModel> products, string sortElement, bool sortDesc)
        {
            sortElement = sortElement.ToLower();

            if (sortElement == SortAndFilterConstants.Name)
            {
                if (sortDesc)
                {
                    return products.OrderByDescending(p => p.Name).ToArray();
                }
                else
                {
                    return products.OrderBy(p => p.Name).ToArray();
                }
            }

            if (sortElement == SortAndFilterConstants.Number)
            {
                if (sortDesc)
                {
                    return products.OrderByDescending(p => p.Number).ToArray();
                }
                else
                {
                    return products.OrderBy(p => p.Number).ToArray();
                }
            }

            if (sortElement == SortAndFilterConstants.Price)
            {
                if (sortDesc)
                {
                    return products.OrderByDescending(p => p.Price).ToArray();
                }
                else
                {
                    return products.OrderBy(p => p.Price).ToArray();
                }
            }

            return products;
        }

        private async Task<ICollection<ProductDetailsModel>> FilterSubcategories(ICollection<ProductDetailsModel> products, ICollection<string> subcategories)
        {
            ICollection<ProductDetailsModel> result = new List<ProductDetailsModel>();

            foreach (ProductDetailsModel product in products)
            {
                if (product.Subcategories != null && product.Subcategories.Any(sc => subcategories.Contains(sc.Id)))
                {
                    result.Add(product);
                }
            }

            return result;
        }

        private async Task<ICollection<ProductDetailsModel>> FilterCategories(ICollection<ProductDetailsModel> products, ICollection<string> categories)
        {
            ICollection<ProductDetailsModel> result = new List<ProductDetailsModel>();

            foreach (ProductDetailsModel product in products)
            {
                if (product.Categories != null && product.Categories.Select(c => c.Id).Any(cid => categories.Contains(cid)))
                {
                    result.Add(product);
                }
            }

            return result;
        }

        #endregion

        #region "Images"

        private async Task CreateImages(IList<string> imageUrls, string productId)
        {
            if (imageUrls != null && imageUrls.Count > 0)
            {
                string[] urls = imageUrls.ToArray();

                for (int i = 0; i < urls.Length; i++)
                {
                    await this.images.Create(urls[i], productId);
                }
            }

            await this.db.SaveChangesAsync();
        }

        private async Task DeleteImages(string productid)
        {
            string[] imageIds = this.db.Images
                .Where(i => i.ProductId == productid)
                .Select(i => i.Id)
                .ToArray();

            for (int i = 0; i < imageIds.Length; i++)
            {
                await this.images.Delete(imageIds[i]);
            }
        }

        #endregion

        #region "Categories"

        private async Task AddToCategories(ICollection<string> categories, string productId)
        {
            if (categories.Count > 0)
            {
                string[] categoryIds = categories.ToArray();

                for (int i = 0; i < categoryIds.Length; i++)
                {
                    CategoryProduct categoryProduct = new CategoryProduct
                    {
                        CategoryId = categoryIds[i],
                        ProductId = productId

                    };

                    await this.db.CategoryProducts.AddAsync(categoryProduct);

                    await this.db.SaveChangesAsync();
                }
            }
            else
            {
                string defaultCategoryId = await this.categories.SeedDefaultCategory();

                CategoryProduct categoryProduct = new CategoryProduct
                {
                    CategoryId = defaultCategoryId,
                    ProductId = productId
                };

                await this.db.CategoryProducts.AddAsync(categoryProduct);

                await this.db.SaveChangesAsync();
            }
        }

        private async Task UpdateCategories(ICollection<string> categories, string productId)
        {
            var categoryProducts = this.db.CategoryProducts.Where(cp => cp.ProductId == productId).ToList();

            this.db.CategoryProducts.RemoveRange(categoryProducts);

            await this.db.SaveChangesAsync();

            await this.AddToCategories(categories, productId);
        }

        #endregion

        #region "Subcategories"

        private async Task AddToSubcategories(ICollection<string> subcategories, string productId)
        {
            if (subcategories != null && subcategories.Count > 0)
            {
                string[] subcategoryIds = subcategories.ToArray();

                for (int i = 0; i < subcategoryIds.Length; i++)
                {
                    SubcategoryProduct subcategoryProduct = new SubcategoryProduct
                    {
                        SubcategoryId = subcategoryIds[i],
                        ProductId = productId

                    };

                    await this.db.SubcategoryProducts.AddAsync(subcategoryProduct);

                    await this.db.SaveChangesAsync();
                }
            }
        }

        private async Task UpdateSubcategories(ICollection<string> subcategories, string productId)
        {
            var subcategoryProducts = this.db.SubcategoryProducts.Where(cp => cp.ProductId == productId).ToList();

            this.db.SubcategoryProducts.RemoveRange(subcategoryProducts);

            await this.db.SaveChangesAsync();

            await this.AddToSubcategories(subcategories, productId);
        }

        #endregion
    }
}
