namespace Api.Services.Interfaces
{
    using Api.Models.PromoDiscount;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPromoDiscountService
    {
        Task<string> Create(PromoDiscountCreateModel data);

        Task<ICollection<PromoDiscountDetailsModel>> GetList();

        Task Assign(string promoDiscountId, string productId);
    }
}
