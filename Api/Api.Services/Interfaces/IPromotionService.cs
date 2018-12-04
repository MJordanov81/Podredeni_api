namespace Api.Services.Interfaces
{
    using Api.Models.Cart;
    using Api.Models.Promotion;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPromotionService
    {
        Task<string> Create(PromotionCreateEditModel data);

        Task<CartPromotionResultModel> CalculatePromotion(CartPromotionCheckModel data);

        Task Delete(string promotionId);

        Task<PromotionDetailsModel> Get(string promotionId);

        Task<ICollection<PromotionDetailsModel>> Get();

        Task Edit(string promotionId, PromotionCreateEditModel data);
    }
}
