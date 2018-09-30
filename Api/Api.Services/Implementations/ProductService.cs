namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Product;
    using Api.Models.Shared;
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
        private readonly ApiDbContext db;

        public ProductService(INumeratorService numerator, IImageService images, ICategoryService categories, ApiDbContext db)
        {
            this.numerator = numerator;
            this.images = images;
            this.categories = categories;
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

            return product;
        }

        //Get details for a range of products - filtered, sorted and paginated
        public async Task<ProductDetailsListPaginatedModel> GetAll(PaginationModel pagination, bool includeBlocked)
        {
 
            IEnumerable<ProductDetailsModel> products = db.Products
                .ProjectTo<ProductDetailsModel>()
                .ToList();

            foreach (var product in products)
            {
                product.PromoDiscountsIds = await this.GetAssociatedPromoDiscuntsIds(product.Id);
            }

            foreach (ProductDetailsModel product in products)
            {
                product.Discount = await this.CalculateDiscount(product.Id);
   
            }

            if (!string.IsNullOrEmpty(pagination.FilterElement))
            {
                products = await this.FilterElements(products, pagination.FilterElement, pagination.FilterValue);
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

            return new ProductDetailsListPaginatedModel
            {
                Products = products,
                ProductsCount = productsCount
            };
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

        private async Task<IEnumerable<ProductDetailsModel>> FilterElements(IEnumerable<ProductDetailsModel> products, string filterElement, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                filterElement = filterElement.ToLower();
                filterValue = filterValue.ToLower();

                if (filterElement == SortAndFilterConstants.Name)
                {
                    return products.Where(p => p.Name.ToLower().Contains(filterValue));
                }

                else if (filterElement == SortAndFilterConstants.Number)
                {
                    return products.Where(p => p.Number.ToString().Contains(filterValue));
                }

                else if (filterElement == SortAndFilterConstants.Quantity)
                {
                    bool isANumber = int.TryParse(filterValue, out int quantity);

                    if (isANumber) return products.Where(p => p.Quantity == quantity);
                }

                else if (filterElement == SortAndFilterConstants.IsBlocked)
                {
                    bool isBoolean = bool.TryParse(filterValue, out bool isBlocked);

                    if (isBoolean) return products.Where(p => p.IsBlocked == isBlocked);
                }


                else if (filterElement == SortAndFilterConstants.IsTopSeller)
                {
                    bool isBoolean = bool.TryParse(filterValue, out bool isTopSeller);

                    if (isBoolean) return products.Where(p => p.IsTopSeller == isTopSeller);
                }
            }

            return products;
        }

        private async Task<IEnumerable<ProductDetailsModel>> SortElements(IEnumerable<ProductDetailsModel> products, string sortElement, bool sortDesc)
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
            if (categories != null && categories.Count > 0)
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
    }
}
