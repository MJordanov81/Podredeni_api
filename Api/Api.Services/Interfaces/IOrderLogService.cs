namespace Api.Services.Interfaces
{
    using Api.Models.OrderLog;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOrderLogService
    {
        Task Log(string orderId, string userId, string action);

        Task<ICollection<OrderLogDetailsModel>> GetLog(string orderId);
    }
}
