namespace Api.Services.Implementations
{
    using System.Threading.Tasks;
    using Api.Data;
    using Api.Models.Partner;
    using Api.Services.Interfaces;
    using System;
    using Api.Services.Infrastructure.Constants;
    using Api.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;

    public class PartnerService : IPartnerService
    {
        private readonly ApiDbContext db;

        public PartnerService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(PartnerCreateEditModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Details))
                throw new ArgumentException(ErrorMessages.InvalidPartnerCreateData);

            Partner partner = new Partner
            {
                Name = data.Name,
                LogoUrl = !string.IsNullOrWhiteSpace(data.LogoUrl) ? data.LogoUrl : "",
                WebUrl = !string.IsNullOrWhiteSpace(data.WebUrl) ? data.WebUrl : "",
                Details = data.Details
            };

            await this.db.Partners.AddAsync(partner);

            await this.db.SaveChangesAsync();

            return partner.Id;
        }

        public async Task Edit(string partnerId, PartnerCreateEditModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Details))
                throw new ArgumentException(ErrorMessages.InvalidPartnerCreateData);

            if(!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            Partner partner = await this.db.Partners.FirstOrDefaultAsync(p => p.Id == partnerId);

            partner.Name = data.Name;
            partner.LogoUrl = data.LogoUrl;
            partner.WebUrl = data.WebUrl;
            partner.Details = data.Details;

            await this.db.SaveChangesAsync();
        }

        public async Task<PartnerDetailsModel> Get(string partnerId)
        {
            if (!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            return await this.db.Partners
                .Where(p => p.Id == partnerId)
                .ProjectTo<PartnerDetailsModel>()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PartnerDetailsModel>> Get()
        {
            return await this.db.Partners
                .ProjectTo<PartnerDetailsModel>()
                .ToListAsync();
        }

        public async Task Delete(string partnerId)
        {
            if (!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            Partner partner = await this.db.Partners.FirstOrDefaultAsync(p => p.Id == partnerId);

            this.db.Partners.Remove(partner);

            await this.db.SaveChangesAsync();
        }
    }
}
