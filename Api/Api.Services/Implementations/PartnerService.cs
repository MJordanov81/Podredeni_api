namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Partner;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PartnerService : IPartnerService
    {
        private readonly ApiDbContext db;

        public PartnerService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(PartnerCreateEditModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Category))
                throw new ArgumentException(ErrorMessages.InvalidPartnerCreateData);

            await this.ReindexPartners(); //Increment each partner's index with 1, so that the new partner comes first

            Partner partner = new Partner
            {
                Name = data.Name,
                LogoUrl = !string.IsNullOrWhiteSpace(data.LogoUrl) ? data.LogoUrl : "",
                WebUrl = !string.IsNullOrWhiteSpace(data.WebUrl) ? data.WebUrl : "",
                Category = data.Category,
                Index = 1
            };

            await this.db.Partners.AddAsync(partner);

            await this.db.SaveChangesAsync();

            if (data.PartnerLocations.Count > 0)
            {
                await this.CreatePartnerLocations(partner.Id, data.PartnerLocations);
            }

            return partner.Id;
        }

        public async Task Edit(string partnerId, PartnerCreateEditModel data)
        {
            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.Category))
                throw new ArgumentException(ErrorMessages.InvalidPartnerCreateData);

            if (!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            Partner partner = await this.db.Partners.FirstOrDefaultAsync(p => p.Id == partnerId);

            partner.Name = data.Name;
            partner.LogoUrl = data.LogoUrl;
            partner.WebUrl = data.WebUrl;
            partner.Category = data.Category;

            await this.db.SaveChangesAsync();

            await this.DeletePartnerLocations(partner.Id);

            if (data.PartnerLocations.Count > 0) await this.CreatePartnerLocations(partner.Id, data.PartnerLocations);
        }

        public async Task<PartnerDetailsModel> Get(string partnerId)
        {
            if (!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            return this.db.Partners
                .Where(p => p.Id == partnerId)
                .ProjectTo<PartnerDetailsModel>()
                .FirstOrDefault();
        }


        public async Task<IEnumerable<PartnerDetailsModel>> Get()
        {
            return this.db.Partners
                .OrderBy(p => p.Index)
                .ProjectTo<PartnerDetailsModel>()
                .ToList();
        }

        public async Task<Dictionary<string, List<PartnerDetailsModel>>> GetGoupedByCity()
        {
            //await this.IndexatePartners();

            SortedSet<string> sortedCities =
                new SortedSet<string>(this.db.PartnerLocations.Select(pl => pl.City).ToHashSet());

            Dictionary<string, List<PartnerDetailsModel>> result = new Dictionary<string, List<PartnerDetailsModel>>();

            foreach (string city in sortedCities)
            {
                result.Add(city, new List<PartnerDetailsModel>());
            }

            result.Add("n/a", new List<PartnerDetailsModel>());

            ICollection<PartnerDetailsModel> partners = this.db.Partners
                .OrderBy(p => p.Index)
                .ProjectTo<PartnerDetailsModel>()
                .ToList();

            foreach (PartnerDetailsModel partner in partners)
            {
                HashSet<string> partnerCities = partner.PartnerLocations.Select(pl => pl.City).ToHashSet();

                foreach (string city in partnerCities)
                {
                    result[city].Add(new PartnerDetailsModel()
                    {
                        Id = partner.Id,
                        Name = partner.Name,
                        WebUrl = partner.WebUrl,
                        LogoUrl = partner.LogoUrl,
                        Category = partner.Category,
                        PartnerLocations = partner.PartnerLocations.Where(pl => pl.City == city).ToList()
                    });
                }

                if (partnerCities.Count == 0)
                {
                    result["n/a"].Add(new PartnerDetailsModel
                    {
                        Id = partner.Id,
                        Name = partner.Name,
                        WebUrl = partner.WebUrl,
                        LogoUrl = partner.LogoUrl,
                        Category = partner.Category
                    });
                }
            }

            if (result["n/a"].Count == 0)
            {
                result.Remove("n/a");
            }

            return result;
        }

        public async Task Delete(string partnerId)
        {
            if (!await this.db.Partners.AnyAsync(p => p.Id == partnerId))
                throw new ArgumentException(ErrorMessages.InvalidPartnerId);

            Partner partner = await this.db.Partners.FirstOrDefaultAsync(p => p.Id == partnerId);

            await this.DeletePartnerLocations(partner.Id);

            this.db.Partners.Remove(partner);

            await this.db.SaveChangesAsync();
        }

        private async Task CreatePartnerLocations(string partnerId, ICollection<PartnerLocationCreateModel> partnerLocations)
        {
            foreach (PartnerLocationCreateModel partnerLocation in partnerLocations)
            {
                if (string.IsNullOrWhiteSpace(partnerLocation.City) || string.IsNullOrWhiteSpace(partnerLocation.Address)) throw new ArgumentException(ErrorMessages.InvalidPartnerLocationCreationData);

                await this.db.PartnerLocations.AddAsync(new PartnerLocation
                {
                    City = partnerLocation.City,
                    Address = partnerLocation.Address,
                    PartnerId = partnerId
                });

                await this.db.SaveChangesAsync();
            }
        }

        private async Task DeletePartnerLocations(string partnerId)
        {
            ICollection<PartnerLocation> partnerLocations = await this.db.PartnerLocations
                .Where(pl => pl.PartnerId == partnerId)
                .ToListAsync();

            this.db.RemoveRange(partnerLocations);

            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Re-index partners according to the order of input array of partners ids
        /// </summary>
        /// <param name="orderedPartnersIds"></param>
        /// <returns></returns>
        public async Task Reorder(string[] orderedPartnersIds)
        {
            IEnumerable<Partner> partners = await this.db.Partners.ToListAsync();

            foreach (Partner partner in partners)
            {
                int index = Array.IndexOf(orderedPartnersIds, partner.Id);

                partner.Index = ++index;
            }

            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Increments each partner's id with 1
        /// </summary>
        /// <returns></returns>
        private async Task ReindexPartners()
        {
            IEnumerable<Partner> partners = await this.db.Partners.ToListAsync();

            foreach (Partner partner in partners)
            {
                partner.Index++;
            }

            await this.db.SaveChangesAsync();
        }

        private async Task IndexatePartners()
        {
            IEnumerable<Partner> partners = await this.db.Partners.ToListAsync();

            int index = 1;

            foreach (Partner partner in partners)
            {
                partner.Index = index++;
            }

            await this.db.SaveChangesAsync();
        }

    }
}
