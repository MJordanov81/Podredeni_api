namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.Settings;
    using Api.Services.Interfaces;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Reflection;
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
            if (!await this.db.Settings.AnyAsync())
            {
                Settings settings = new Settings();

                await this.db.Settings.AddAsync(settings);

                await this.db.SaveChangesAsync();
            }

            return await this.db.Settings.ProjectTo<SettingsViewEditModel>().FirstOrDefaultAsync();
        }

        public async Task Update(SettingsViewEditModel data)
        {
            if (!await this.db.Settings.AnyAsync())
            {
                throw new InvalidOperationException();
            }

            Settings settings = await this.db.Settings.FirstOrDefaultAsync();

            settings = await this.UpdateModel(data, settings);

            await this.db.SaveChangesAsync();
        }

        private async Task<Settings> UpdateModel(SettingsViewEditModel settingsNew, Settings settingsOld)
        {
            settingsOld.ShowOutOfStock = settingsNew.ShowOutOfStock;

            return settingsOld;
        }
    }
}
