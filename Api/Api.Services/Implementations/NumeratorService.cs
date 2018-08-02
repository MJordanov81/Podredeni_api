namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Infrastructure.Constants;
    using Interfaces;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class NumeratorService : INumeratorService
    {
        private readonly ApiDbContext db;

        public NumeratorService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<int> GetNextNumer(Type entityType)
        {
            if (entityType == null) throw new ArgumentException(ErrorMessages.InvalidType);

            Numerator numerator = this.db.Numerator.FirstOrDefault();

            if(numerator == null)
            {
                numerator = new Numerator();

                try
                {
                    await this.db.Numerator.AddAsync(numerator);

                    await this.db.SaveChangesAsync();
                }
                catch
                {
                    throw new InvalidOperationException(ErrorMessages.UnableToWriteToDb);
                }
            }

            string nameOfType = entityType.Name;

            PropertyInfo property = numerator.GetType().GetProperty($"{nameOfType}CurrentValue");

            int currentNumber = (int)property.GetValue(numerator, null);

            property.SetValue(numerator, ++currentNumber);

            await this.db.SaveChangesAsync();

            return currentNumber;
        }
    }
}
