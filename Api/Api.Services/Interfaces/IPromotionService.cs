namespace Api.Services.Interfaces
{
    using Api.Models.Promotion;
    using System.Threading.Tasks;

    public interface IPromotionService
    {
        Task<string> Create(PromotionCreateEditModel data);
    }
}
