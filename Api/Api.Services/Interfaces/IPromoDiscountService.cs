namespace Api.Services.Interfaces
{
    using Api.Models.PromoDiscount;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPromoDiscountService
    {
        Task<string> Create(PromoDiscountCreateModel data);

        Task<ICollection<PromoDiscountDetailsModel>> GetList();

        Task<PromoDiscountDetailsModel> Get(string promoId);

        Task Assign(string promoId, string productId);

        Task Remove(string promoId, string productId);

        Task Edit(string promoId, PromoDiscountCreateModel data);

        Task Delete(string promoId);
    }
}
