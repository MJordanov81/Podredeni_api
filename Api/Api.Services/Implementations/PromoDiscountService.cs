namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.PromoDiscount;
    using AutoMapper.QueryableExtensions;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PromoDiscountService : IPromoDiscountService
    {
        private readonly ApiDbContext db;

        public PromoDiscountService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task Assign(string promoDiscountId, string productId)
        {
            if (!await this.db.PromoDiscounts.AnyAsync(d => d.Id == promoDiscountId) || !await this.db.Products.AnyAsync(p => p.Id == productId))
            {
                throw new ArgumentException("Either promo or product not found in DB");
            }

            ProductPromoDiscount existingProductPromoDiscount = await this.db.ProductPromoDiscounts.FirstOrDefaultAsync(d => d.PromoDiscountId == promoDiscountId && d.ProductId == productId);

            if (existingProductPromoDiscount != null)
            {
                this.db.ProductPromoDiscounts.Remove(existingProductPromoDiscount);

                await this.db.SaveChangesAsync();

                return;
            }

            ProductPromoDiscount productPromoDiscount = new ProductPromoDiscount
            {
                PromoDiscountId = promoDiscountId,
                ProductId = productId
            };

            await this.db.ProductPromoDiscounts.AddAsync(productPromoDiscount);

            await this.db.SaveChangesAsync();
        }

        public async Task<string> Create(PromoDiscountCreateModel data)
        {
            if (data.Discount < 0 || data.Discount > 100) throw new ArgumentException("Discount must be between 0 and 100");

            PromoDiscount discount = new PromoDiscount
            {
                Name = data.Name,
                Discount = data.Discount,
                StartDate = data.StartDate,
                EndDate = data.EndDate
            };

            await this.db.PromoDiscounts
                .AddAsync(discount);

            await this.db.SaveChangesAsync();

            return discount.Id;
        }

        public async Task<ICollection<PromoDiscountDetailsModel>> GetList()
        {
            return await this.db.PromoDiscounts
                .ProjectTo<PromoDiscountDetailsModel>()
                .ToListAsync();
        }
    }
}
