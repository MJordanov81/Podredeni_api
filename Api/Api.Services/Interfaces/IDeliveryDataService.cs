namespace Api.Services.Interfaces
{
    using Api.Models.DeliveryData;
    using System.Threading.Tasks;

    public interface IDeliveryDataService
    {
        Task<string> Create(
            DeliveryDataCreateModel data);

        Task<DeliveryDataDetailsModel> Get(string id);

        Task<string> Edit(string deliveryDataId, DeliveryDataCreateModel data);
    }
}
