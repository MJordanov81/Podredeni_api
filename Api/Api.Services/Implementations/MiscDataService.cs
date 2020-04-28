namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MiscDataService : IMiscDataService
    {
        private readonly ApiDbContext db;

        public MiscDataService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> CreateOrUpdateAsync(string key, string content)
        {
            if (String.IsNullOrEmpty(key) || String.IsNullOrWhiteSpace(key)) throw new ArgumentException(ErrorMessages.InvalidArguments);

            if (String.IsNullOrEmpty(content)) throw new ArgumentException(ErrorMessages.InvalidArguments);

            MiscData data = await this.db.MiscData
                .Where(d => d.Key == key.ToLower())
                .FirstOrDefaultAsync();

            if (data == null)
            {
                MiscData newData = new MiscData();

                newData.Key = key.ToLower();
                newData.Value = content;

                try
                {
                    await this.db.AddAsync(newData);

                    await this.db.SaveChangesAsync();

                }
                catch (Exception)
                {

                    throw new InvalidOperationException(ErrorMessages.UnableToWriteToDb);
                }

                return newData.Key;

            }
            else
            {
                data.Value = content;

                await this.db.SaveChangesAsync();

                return data.Key;
            }
            
        }

        public async Task<string> GetAsync(string key)
        {
            if (String.IsNullOrEmpty(key) || String.IsNullOrWhiteSpace(key)) throw new ArgumentException(ErrorMessages.InvalidArguments);

            MiscData data = await this.db.MiscData
                .Where(d => d.Key == key.ToLower())
                .FirstOrDefaultAsync();

            if (data == null)
            {
                throw new ArgumentException(ErrorMessages.InvalidArguments);
            }
            else
            {
                return data.Value;
            }

        }
    }
}
