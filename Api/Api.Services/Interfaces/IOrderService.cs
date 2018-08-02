namespace Api.Services.Interfaces
{
    using Api.Domain.Enums;
    using Api.Models.Order;
    using Api.Models.Shared;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<string> Create(OrderWithoutUserCreateModel data, string userId);

        Task<string> Edit(string id, string userId, OrderWithoutUserEditModel data);

        Task<OrderDetailsModel> ChangeStatus(string id, string userId, OrderStatus newStatus);

        Task<bool> IsConfirmationMailSent(string orderId);

        Task SetConfirmationMailSent(string orderId);

        Task<string> GetEmail(string orderId);

        Task<OrderDetailsModel> Get(string orderId);

        Task<OrderDetailsListPaginatedModel> GetAll(PaginationModel pagination);
    }
}
