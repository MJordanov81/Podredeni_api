namespace Api.Services.Interfaces
{
    using Api.Models.Settings;
    using System.Threading.Tasks;

    public interface ISettingsService
    {
        Task<SettingsViewEditModel> Get();

        Task Update(SettingsViewEditModel data);
    }
}
