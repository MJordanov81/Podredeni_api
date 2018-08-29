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
    using System.Linq;
    using System.Threading.Tasks;

    public class PromoDiscountService : IPromoDiscountService
    {
        private readonly ApiDbContext db;

        public PromoDiscountService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task Assign(string promoId, string productId)
        {
            if (!await this.db.PromoDiscounts.AnyAsync(d => d.Id == promoId) || !await this.db.Products.AnyAsync(p => p.Id == productId))
            {
                throw new ArgumentException("Either promo or product not found in DB");
            }

            ProductPromoDiscount existingProductPromoDiscount = await this.db.ProductPromoDiscounts.FirstOrDefaultAsync(d => d.PromoDiscountId == promoId && d.ProductId == productId);

            if (existingProductPromoDiscount == null)
            {
                ProductPromoDiscount productPromoDiscount = new ProductPromoDiscount
                {
                    PromoDiscountId = promoId,
                    ProductId = productId
                };

                await this.db.ProductPromoDiscounts.AddAsync(productPromoDiscount);
            }

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

        public async Task Delete(string promoId)
        {
            if (!await this.db.PromoDiscounts.AnyAsync(p => p.Id == promoId)) throw new ArgumentException("Cannot find promo in DB");

            IList<ProductPromoDiscount> productPromoDiscounts = await this.db.ProductPromoDiscounts
                .Where(p => p.PromoDiscountId == promoId)
                .ToListAsync();

            this.db.ProductPromoDiscounts.RemoveRange(productPromoDiscounts);

            await this.db.SaveChangesAsync();

            PromoDiscount promo = await this.db.PromoDiscounts.FirstOrDefaultAsync(p => p.Id == promoId);

            this.db.PromoDiscounts.Remove(promo);

            await this.db.SaveChangesAsync();
        }

        public async Task Edit(string promoId, PromoDiscountCreateModel data)
        {
            if (data.Discount < 0 || data.Discount > 100) throw new ArgumentException("Discount must be between 0 and 100");

            if (!await this.db.PromoDiscounts.AnyAsync(p => p.Id == promoId)) throw new ArgumentException("Cannot find promo in DB");

            PromoDiscount promo = await this.db.PromoDiscounts.FirstOrDefaultAsync(p => p.Id == promoId);

            promo.Name = data.Name;
            promo.Discount = data.Discount;
            promo.EndDate = data.EndDate;
            promo.StartDate = data.StartDate;

            await this.db.SaveChangesAsync();
        }

        public async Task<PromoDiscountDetailsModel> Get(string promoId)
        {
            if (!await this.db.PromoDiscounts.AnyAsync(d => d.Id == promoId)) throw new ArgumentException("Cannot find promo in DB");

            PromoDiscountDetailsModel model = await this.db.PromoDiscounts
                .Where(d => d.Id == promoId)
                .ProjectTo<PromoDiscountDetailsModel>()
                .FirstOrDefaultAsync();

            model.ProductsIds = await this.GetAssociatedProductsIds(promoId);

            return model;
        }

        public async Task<ICollection<PromoDiscountDetailsModel>> GetList()
        {
            ICollection<PromoDiscountDetailsModel> models = await this.db.PromoDiscounts
                .ProjectTo<PromoDiscountDetailsModel>()
                .ToListAsync();

            foreach (PromoDiscountDetailsModel model in models)
            {
                model.ProductsIds = await this.GetAssociatedProductsIds(model.Id);
            }

            return models;
        }

        private async Task<ICollection<string>> GetAssociatedProductsIds(string promoId)
        {
            return await this.db.ProductPromoDiscounts
                .Where(pd => pd.PromoDiscountId == promoId)
                .Select(pd => pd.ProductId)
                .ToListAsync();
        }

        public async Task Remove(string promoId, string productId)
        {
            if (!await this.db.PromoDiscounts.AnyAsync(d => d.Id == promoId) || !await this.db.Products.AnyAsync(p => p.Id == productId))
            {
                throw new ArgumentException("Either promo or product not found in DB");
            }

            ProductPromoDiscount existingProductPromoDiscount = await this.db.ProductPromoDiscounts.FirstOrDefaultAsync(d => d.PromoDiscountId == promoId && d.ProductId == productId);

            if (existingProductPromoDiscount != null)
            {
                this.db.ProductPromoDiscounts.Remove(existingProductPromoDiscount);
            }

            await this.db.SaveChangesAsync();
        }
    }
}
