namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Cart;
    using Api.Models.Product;
    using Api.Models.Promotion;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PromotionService : IPromotionService
    {
        private readonly ApiDbContext db;

        private readonly IProductService products;

        public PromotionService(ApiDbContext db, IProductService products)
        {
            this.db = db;
            this.products = products;
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

            if (this.db.Promotions.Any(p => p.PromoCode == data.PromoCode)) throw new ArgumentException(ErrorMessages.InvalidPromotionPromoCode);

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

        public async Task<object> ManagePromotion(CartPromotionCheckModel data)
        {
            IQueryable<Promotion> promotions = this.db.Promotions.Where(p => p.PromoCode == data.PromoCode);

            string promoId = (await promotions.FirstOrDefaultAsync()).Id;

            int selectableDiscountedProductsCount = promotions
                .Select(p => p.DiscountedProductsPromotions.Count)
                .FirstOrDefault();

            int discountedProductsCount = promotions
                .FirstOrDefault()
                .DiscountedProductsCount;

            if (selectableDiscountedProductsCount
                >
                discountedProductsCount)
            {
                return await this.ReturnSelectableDiscountedProducts(promoId);
            }
            else
            {
                return await this.CalculatePromotion(data);
            }
        }

        public async Task Delete(string promotionId)
        {
            Promotion promotion = await this.db.Promotions.FirstOrDefaultAsync(p => p.Id == promotionId);

            if (promotion.UsedQuota > 0) throw new InvalidOperationException(ErrorMessages.InvalidDeleteRequest);

            else
            {
                this.db.Promotions.Remove(promotion);

                await this.db.SaveChangesAsync();
            }
        }

        public async Task Edit(string promotionId, PromotionCreateEditModel data)
        {
            Promotion promotion = await this.db.Promotions.FirstOrDefaultAsync(p => p.Id == promotionId);

            if (data.StartDate > data.EndDate) throw new ArgumentException(ErrorMessages.InvalidPromotionDates);

            if (data.DiscountedProducts.Count < 1 || data.Products.Count < 1) throw new ArgumentException(ErrorMessages.InvalidPromotionProductsCount);

            if (data.IsInclusive && data.ProductsCount < data.DiscountedProductsCount) throw new ArgumentException(ErrorMessages.InvalidPromotionProductsCount);

            if (this.db.Promotions.Any(p => p.PromoCode == data.PromoCode) && data.PromoCode != promotion.PromoCode) throw new ArgumentException(ErrorMessages.InvalidPromotionPromoCode);

            await CheckProductIdsInDb(data.Products);

            await CheckProductIdsInDb(data.DiscountedProducts);

            if (promotion == null) throw new ArgumentException(ErrorMessages.InvalidPromotionId);

            promotion.Name = data.Name;
            promotion.StartDate = data.StartDate;
            promotion.EndDate = data.EndDate;
            promotion.PromoCode = data.PromoCode;
            promotion.IsInclusive = data.IsInclusive;
            promotion.IsAccumulative = data.IsAccumulative;
            promotion.ProductsCount = data.ProductsCount;
            promotion.Discount = data.Discount;
            promotion.Quota = data.Quota;

            await this.db.SaveChangesAsync();

            await this.DeleteRelatedData(promotion.Id);

            await AddProductPromotionsToDb<ProductPromotion>(data.Products, promotion.Id);

            await AddProductPromotionsToDb<DiscountedProductPromotion>(data.DiscountedProducts, promotion.Id);
        }

        private async Task DeleteRelatedData(string promotionId)
        {
            IQueryable<ProductPromotion> productPromotions = this.db.ProductsPromotions.Where(pp => pp.PromotionId == promotionId);

            this.db.ProductsPromotions.RemoveRange(productPromotions);

            IQueryable<DiscountedProductPromotion> discountedProductPromotions = this.db.DiscountedProductsPromotions.Where(dpp => dpp.PromotionId == promotionId);

            this.db.DiscountedProductsPromotions.RemoveRange(discountedProductPromotions);

            await this.db.SaveChangesAsync();
        }

        public async Task<PromotionDetailsModel> Get(string promotionId)
        {
            if (!await this.db.Promotions.AnyAsync(p => p.Id == promotionId)) throw new ArgumentException(ErrorMessages.InvalidPromotionId);

            PromotionDetailsModel promotion = this.db.Promotions
                .Where(p => p.Id == promotionId)
                .ProjectTo<PromotionDetailsModel>()
                .FirstOrDefault();

            return promotion;
        }

        public async Task<ICollection<PromotionDetailsModel>> Get()
        {
            ICollection<PromotionDetailsModel> promotions = this.db.Promotions
                .ProjectTo<PromotionDetailsModel>()
                .ToList();

            return promotions;
        }

        private async Task<ICollection<ProductDetailsModel>> ReturnSelectableDiscountedProducts(string promoId)
        {
            IList<string> selectableProductsIds = this.db.DiscountedProductsPromotions
                .Where(p => p.PromotionId == promoId)
                .Select(p => p.ProductId)
                .ToList();

            var result = this.db.Products
                .Where(p => selectableProductsIds.Contains(p.Id))
                .ProjectTo<ProductDetailsModel>()
                .ToList();

            return result;
        }

        private async Task<CartPromotionResultModel> CalculatePromotion(CartPromotionCheckModel data)
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

            foreach (ProductInCartModel product in data.Products)
            {
                if (productsInPromotion.Contains(product.Id))
                {
                    productsInPromotionCount += product.Quantity;
                }
            }

            IList<string> productsInPromotionFromCart = data.Products
                .Select(p => p.Id)
                .ToList();

            if (!await this.IsProductsCountConditionMet(productsInPromotionCount, promotion.ProductsCount)) throw new ArgumentException("No promotion available for selected products/ products quantity");

            if (promotion.IsInclusive) return await this.CaclulateInclusivePromotion(promotion, data, productsInPromotionCount, productsInPromotionFromCart);

            else return await CalculateNotInclusivePromotion(promotion, data, productsInPromotionCount, productsInPromotionFromCart);
        }

        private async Task<CartPromotionResultModel> CalculateNotInclusivePromotion(Promotion promotion, CartPromotionCheckModel data, int productsInPromotionCount, IList<string> productsInPromotionFromCart)
        {
            CartPromotionResultModel result = new CartPromotionResultModel
            {
                Products = new List<ProductInCartModel>()
            };

            int quantityToBeGivenAsPromotion = promotion.IsAccumulative
                ? (productsInPromotionCount / promotion.ProductsCount)
                : 1;

            bool includePriceDiscounts = promotion.IncludePriceDiscounts;

            decimal promotionDiscount = promotion.Discount;

            IList<string> freeProductIds = await this.db.DiscountedProductsPromotions
                .Where(d => d.PromotionId == promotion.Id)
                .Select(p => p.ProductId)
                .ToListAsync();

            foreach (string id in freeProductIds)
            {
                ProductDetailsModel currentProduct = await this.products.Get(id);

                ProductInCartModel promotions = new ProductInCartModel
                {
                    Id = id,
                    Name = currentProduct.Name,
                    ImageUrl = currentProduct.Images.Reverse().FirstOrDefault(),

                    Quantity = quantityToBeGivenAsPromotion,
                    Price = currentProduct.Price,
                    Discount = promotionDiscount
                };

                result.Products.Add(promotions);

                await this.db.SaveChangesAsync();
            }

            foreach (ProductInCartModel product in data.Products)
            {
                ProductDetailsModel currentProduct = await this.products.Get(product.Id);

                decimal discount = currentProduct.Discount;

                if (productsInPromotionFromCart.Contains(currentProduct.Id) && !includePriceDiscounts) discount = 0;

                ProductInCartModel modifiedProduct = new ProductInCartModel
                {
                    Id = product.Id,
                    Name = currentProduct.Name,
                    ImageUrl = currentProduct.Images
                    .Reverse()
                    .FirstOrDefault(),

                    Quantity = product.Quantity,
                    Price = currentProduct.Price,
                    Discount = discount
                };

                result.Products.Add(modifiedProduct);

            }

            return result;
        }

        private async Task<CartPromotionResultModel> CaclulateInclusivePromotion(Promotion promotion, CartPromotionCheckModel data, int productsInPromotionCount, IList<string> productsInPromotionFromCart)
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

                ProductDetailsModel currentProduct = await this.products.Get(product.Id);

                if (remainingQuantityToBeGivenAsPromotion > 0 && productsInPromotionFromCart.Contains(product.Id))
                {
                    if (!includePriceDiscounts) promotionDisount = 0;

                    if (product.Quantity > remainingQuantityToBeGivenAsPromotion)
                    {
                        ProductInCartModel discounted = new ProductInCartModel
                        {
                            Id = product.Id,
                            Name = currentProduct.Name,
                            ImageUrl = currentProduct.Images
                                .Reverse()
                                .FirstOrDefault(),

                            Quantity = remainingQuantityToBeGivenAsPromotion,
                            Price = currentProduct.Price,
                            Discount = promotionDisount
                        };

                        ProductInCartModel productNotDiscounted = new ProductInCartModel
                        {
                            Id = product.Id,
                            Name = currentProduct.Name,
                            ImageUrl = currentProduct.Images
                                .Reverse()
                                .FirstOrDefault(),

                            Quantity = product.Quantity - remainingQuantityToBeGivenAsPromotion,
                            Price = currentProduct.Price,
                            Discount = 0
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
                            Name = currentProduct.Name,
                            ImageUrl = currentProduct.Images
                                .Reverse()
                                .FirstOrDefault(),

                            Quantity = product.Quantity,
                            Price = currentProduct.Price,
                            Discount = promotionDisount
                        };

                        result.Products.Add(discounted);

                        quantityGivenAsPromotion += product.Quantity;
                    }
                }

                else
                {
                    if (productsInPromotionFromCart.Contains(product.Id) && !includePriceDiscounts)
                    {
                        product.Name = currentProduct.Name;

                        product.ImageUrl = currentProduct.Images.Reverse().FirstOrDefault();

                        product.Price = currentProduct.Price;

                        product.Discount = 0;

                        result.Products.Add(product);
                    }
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
