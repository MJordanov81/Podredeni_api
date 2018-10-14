namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Promotion;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PromotionService : IPromotionService
    {
        private readonly ApiDbContext db;

        public PromotionService(ApiDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Creates a promotion with the provided PromotionCreateEditModel data parameters
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The id of the created promotion</returns>
        public async Task<string> Create(PromotionCreateEditModel data)
        {
            if (data.StartDate > data.EndDate) throw new ArgumentException(ErrorMessages.InvalidPromotionDates);

            if (data.DiscountedProducts.Count < 1 || data.Products.Count < 1) throw new ArgumentException(ErrorMessages.InvalidPromotionProductsCount);

            await CheckProductIdsInDb(data.Products);

            await CheckProductIdsInDb(data.DiscountedProducts);

            Promotion promotion = new Promotion
            {
                Name = data.Name,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                PromoCode = data.PromoCode,
                IsInclusive = data.IsInclusive,
                IsAccumulative = data.IsAccumulative,
                ProductsCount = data.ProductsCount,
                DiscountedProductsCount = data.DiscountedProductsCount,
                Discount = data.Discount,
                IncludePriceDiscounts = data.IncludePriceDiscounts,
                Quota = data.Quota
            };

            await this.db.Promotions.AddAsync(promotion);

            await this.db.SaveChangesAsync();

            await AddProductPromotionsToDb<ProductPromotion>(data.Products, promotion.Id);

            await AddProductPromotionsToDb<DiscountedProductPromotion>(data.DiscountedProducts, promotion.Id);

            return promotion.Id;
        }

        /// <summary>
        /// Adds product-promotion pairs to the database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="productIds">A collection of product ids</param>
        /// <param name="promotionId">A promotion id</param>
        /// <returns></returns>
        private async Task AddProductPromotionsToDb<T>(ICollection<string> productIds, string promotionId)
        {
            if (typeof(T) == typeof(ProductPromotion))
            {
                ICollection<ProductPromotion> productPromotions = new List<ProductPromotion>();

                foreach (string productId in productIds)
                {
                    ProductPromotion productPromotion = new ProductPromotion
                    {
                        ProductId = productId,
                        PromotionId = promotionId
                    };

                    productPromotions.Add(productPromotion);
                }

                await this.db.ProductsPromotions.AddRangeAsync(productPromotions);

                await this.db.SaveChangesAsync();
            }
            else
            {
                ICollection<DiscountedProductPromotion> productPromotions = new List<DiscountedProductPromotion>();

                foreach (string productId in productIds)
                {
                    DiscountedProductPromotion productPromotion = new DiscountedProductPromotion
                    {
                        ProductId = productId,
                        PromotionId = promotionId
                    };

                    productPromotions.Add(productPromotion);
                }

                await this.db.DiscountedProductsPromotions.AddRangeAsync(productPromotions);

                await this.db.SaveChangesAsync();
            }
        }

       /// <summary>
       /// Checks if the input ids are existent in the database
       /// </summary>
       /// <param name="products">A collection of product ids</param>
       /// <returns></returns>
        private async Task CheckProductIdsInDb(ICollection<string> products)
        {
            foreach (string productId in products)
            {
                if (!this.db.Products.Any(p => p.Id == productId)) throw new ArgumentException("Cannot find a product with ID " + productId);
            }
        }
    }
}
