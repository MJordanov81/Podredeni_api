namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    class ProductPlacementByCategoryService : IProductPlacementService
    {
        private readonly ApiDbContext db;

        public ProductPlacementByCategoryService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task ChangePlace(string productId, string categoryId, int newPlace)
        {
            await this.ReorderProductInCategory(categoryId, newPlace);

            CategoryProduct cp = await this.db.CategoryProducts.FirstOrDefaultAsync(cpr => cpr.CategoryId == categoryId && cpr.ProductId == productId);

            cp.Place = newPlace;

            await this.db.SaveChangesAsync();
        }

        public async Task SetInitialPlace(string productId, ICollection<string> categories, bool isFirstProduct)
        {
            if (!await this.db.Products.AnyAsync(p => p.Id == productId)) throw new ArgumentException(ErrorMessages.InvalidProductId);

            if (categories.Count < 1) return;

            foreach (string category in categories)
            {               
                CategoryProduct cp = await this.db.CategoryProducts
                    .FirstOrDefaultAsync(cpr => cpr.ProductId == productId && cpr.CategoryId == category);

                if (isFirstProduct)
                {
                    await this.ReorderProductInCategory(category, 1);

                    cp.Place = 1;
                } else
                {
                    cp.Place = this.db.CategoryProducts.Where(cpr => cpr.CategoryId == category).Count() + 1;
                }
                
            }
            
            await this.db.SaveChangesAsync();
        }

        private async Task ReorderProductInCategory(string categoryId, int fromPlace)
        {
            await this.db.CategoryProducts.Where(cp => cp.CategoryId == categoryId).Where(cp => cp.Place >= fromPlace).ForEachAsync(cp => cp.Place++);

            await this.db.SaveChangesAsync();
        }
    }
}
