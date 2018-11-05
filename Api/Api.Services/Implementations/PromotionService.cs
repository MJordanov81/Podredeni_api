namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Data.Migrations;
    using Api.Domain.Entities;
    using Api.Models.Cart;
    using Api.Models.Promotion;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
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

            if (data.IsInclusive && data.ProductsCount < data.DiscountedProductsCount) throw new ArgumentException(ErrorMessages.InvalidPromotionProductsCount);

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

        public async Task<CartPromotionResultModel> CalculatePromotion(CartPromotionCheckModel data)
        {
            Promotion promotion = await this.db.Promotions.FirstOrDefaultAsync(p => p.PromoCode == data.PromoCode);

            if (promotion == null)
            {
                throw new ArgumentException("Invalid PromoCode");
            };

            if (!await this.IsActive(promotion.StartDate, promotion.EndDate)) throw new ArgumentException("Promotion is not active");

            if (!await this.IsWithinQuota(promotion.UsedQuota, promotion.Quota)) throw new ArgumentException("Quota exceeded");

            ICollection<string> productsInPromotion = await this.GetProductsInPromotion(data.PromoCode);

            int productsInPromotionCount = 0;

            IDictionary<string, decimal> productsInPromotionFromCart = new Dictionary<string, decimal>();

            foreach (ProductInCartModel product in data.Products)
            {
                if (productsInPromotion.Contains(product.Id))
                {
                    productsInPromotionCount += product.Quantity;

                    if (!productsInPromotionFromCart.ContainsKey(product.Id))
                    {
                        decimal price = (await this.db.Products.FirstOrDefaultAsync(p => p.Id == product.Id)).Price;

                        productsInPromotionFromCart.Add(product.Id, price);
                    }
                }
            }

            if (!await this.IsProductsCountConditionMet(productsInPromotionCount, promotion.ProductsCount)) throw new ArgumentException("No promotion available for selected products/ products quantity");

            if (promotion.IsInclusive) return await this.CaclulateInclusivePromotion(promotion, data, productsInPromotionCount, productsInPromotionFromCart);

            else return await CalculateNotInclusivePromotion(promotion, data, productsInPromotionCount, productsInPromotionFromCart);
        }

        private async Task<CartPromotionResultModel> CalculateNotInclusivePromotion(Promotion promotion, CartPromotionCheckModel data, int productsInPromotionCount, IDictionary<string, decimal> productsInPromotionFromCart)
        {
            CartPromotionResultModel result = new CartPromotionResultModel
            {
                Products = new List<ProductInCartModel>()
            };

            int quantityToBeGivenAsPromotion = promotion.IsAccumulative
                ? (productsInPromotionCount / promotion.ProductsCount) * promotion.DiscountedProductsCount
                : promotion.DiscountedProductsCount;

            bool includePriceDiscounts = promotion.IncludePriceDiscounts;

            decimal promotionDisount = promotion.Discount;

            string freeProductId = await this.db.DiscountedProductsPromotions
                .Where(d => d.PromotionId == promotion.Id)
                .Select(p => p.ProductId)
                .FirstOrDefaultAsync();

            Product freeProductData = await this.db.Products.FirstOrDefaultAsync(p => p.Id == freeProductId);

            ProductInCartModel promotions = new ProductInCartModel
            {
                Id = freeProductId,
                Quantity = quantityToBeGivenAsPromotion,
                Price = freeProductData.Price - freeProductData.Price * (promotionDisount / 100)
            };

            result.Products.Add(promotions);

            foreach (ProductInCartModel product in data.Products)
            {
                decimal price = product.Price;

                if (productsInPromotionFromCart.Keys.Contains(product.Id))
                {
                    if (!includePriceDiscounts) price = productsInPromotionFromCart[product.Id];

                    ProductInCartModel modifiedProduct = new ProductInCartModel
                    {
                        Id = product.Id,
                        Quantity = product.Quantity,
                        Price = price
                    };

                    result.Products.Add(modifiedProduct);
                }

                else
                {
                    result.Products.Add(product);
                }
            }

            return result;
        }

        private async Task<CartPromotionResultModel> CaclulateInclusivePromotion(Promotion promotion, CartPromotionCheckModel data, int productsInPromotionCount, IDictionary<string, decimal> productsInPromotionFromCart)
        {
            CartPromotionResultModel result = new CartPromotionResultModel
            {
                Products = new List<ProductInCartModel>()
            };

            int quantityToBeGivenAsPromotion = promotion.IsAccumulative
                ? (productsInPromotionCount / promotion.ProductsCount) * promotion.DiscountedProductsCount
                : promotion.DiscountedProductsCount;

            int quantityGivenAsPromotion = 0;

            bool includePriceDiscounts = promotion.IncludePriceDiscounts;

            decimal promotionDisount = promotion.Discount;

            foreach (ProductInCartModel product in data.Products)
            {
                int remainingQuantityToBeGivenAsPromotion = quantityToBeGivenAsPromotion - quantityGivenAsPromotion;

                decimal price = product.Price;

                if (remainingQuantityToBeGivenAsPromotion > 0 && productsInPromotionFromCart.Keys.Contains(product.Id))
                {
                    if (!includePriceDiscounts) price = productsInPromotionFromCart[product.Id];

                    if (product.Quantity > remainingQuantityToBeGivenAsPromotion)
                    {
                        ProductInCartModel discounted = new ProductInCartModel
                        {
                            Id = product.Id,
                            Quantity = remainingQuantityToBeGivenAsPromotion,
                            Price = (price - price * (promotionDisount / 100))
                        };

                        ProductInCartModel productNotDiscounted = new ProductInCartModel
                        {
                            Id = product.Id,
                            Quantity = product.Quantity - remainingQuantityToBeGivenAsPromotion,
                            Price = price
                        };

                        result.Products.Add(discounted);

                        result.Products.Add(productNotDiscounted);

                        quantityGivenAsPromotion += remainingQuantityToBeGivenAsPromotion;
                    }

                    else
                    {
                        ProductInCartModel discounted = new ProductInCartModel
                        {
                            Id = product.Id,
                            Quantity = product.Quantity,
                            Price = price - price * (promotionDisount / 100)
                        };

                        result.Products.Add(discounted);

                        quantityGivenAsPromotion += product.Quantity;
                    }
                }

                else
                {
                    if (productsInPromotionFromCart.Keys.Contains(product.Id) && !includePriceDiscounts)
                    {
                        product.Price = productsInPromotionFromCart[product.Id];
                    }

                    result.Products.Add(product);
                }
            }

            return result;
        }

        private async Task<bool> IsPromotionInclusive(string promoCode)
        {
            return (await this.db.Promotions.FirstOrDefaultAsync(p => p.PromoCode == promoCode)).IsInclusive;
        }

        private async Task<bool> IsProductsCountConditionMet(int productsInPromotionCount, int productsCount)
        {
            return productsCount <= productsInPromotionCount;
        }

        private async Task<ICollection<string>> GetProductsInPromotion(string promoCode)
        {
            string promotionId = (await this.db.Promotions.FirstOrDefaultAsync(p => p.PromoCode == promoCode)).Id;

            return await this.db.ProductsPromotions
                .Where(pp => pp.PromotionId == promotionId)
                .Select(p => p.ProductId)
                .ToListAsync();
        }

        private async Task<bool> IsWithinQuota(int usedQuota, int quota)
        {
            return usedQuota < quota;
        }

        private async Task<bool> IsActive(DateTime startDate, DateTime endDate)
        {
            DateTime today = DateTime.Now;

            return today > startDate && today < endDate;
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
