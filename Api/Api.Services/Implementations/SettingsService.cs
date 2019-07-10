namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Settings;
    using Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SettingsService : ISettingsService
    {
        private readonly ApiDbContext db;

        public SettingsService(ApiDbContext db)
        {
            this.db = db;
        }
        public async Task<SettingsViewEditModel> Get()
        {
            SettingsViewEditModel settings = new SettingsViewEditModel();

            settings.Settings = new Dictionary<string, int>();

            if (this.db.Settings.Any())
            {

                this.db.Settings
                    .ToList()
                    .ForEach(s => settings.Settings.Add(s.Name, s.Value));
               
            }

            return settings;
        }

        public async Task Update(SettingsViewEditModel data)
        {
            ICollection<Settings> settings = await this.db.Settings.ToListAsync();

            foreach (var setting in data.Settings)
            {
                if(!settings.Any(s => s.Name == setting.Key))
                {
                   await this.db.AddAsync<Settings>(new Settings { Name = setting.Key, Value = setting.Value });
                }
                else
                {
                    settings.FirstOrDefault(s => s.Name == setting.Key).Value = setting.Value;
                }

                await this.db.SaveChangesAsync();
            }
        }
    }
}
