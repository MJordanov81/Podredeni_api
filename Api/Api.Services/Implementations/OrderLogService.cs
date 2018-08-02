namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Services.Infrastructure.Constants;
    using Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Api.Models.OrderLog;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    public class OrderLogService : IOrderLogService
    {
        private readonly ApiDbContext db;

        public OrderLogService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<OrderLogDetailsModel>> GetLog(string orderId)
        {
            if (!this.db.Orders.Any(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            return this.db.OrderLogs
                   .Where(ol => ol.OrderId == orderId)
                   .OrderBy(ol => ol.DateTime)
                   .ProjectTo<OrderLogDetailsModel>()
                   .ToList();

        }

        public async Task Log(string orderId, string userId, string action)
        {
            if (action == null) throw new ArgumentException(ErrorMessages.InvalidArguments);

            //authentication not implemented yet
            //if (!this.db.Users.Any(u => u.Id == userId)) throw new ArgumentException(ErrorMessages.InvalidUserId);

            if(!this.db.Orders.Any(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            OrderLog log = new OrderLog
            {
                UserId = userId,
                OrderId = orderId,
                DateTime = DateTime.Now,
                Action = action
            };

            await this.db.OrderLogs.AddAsync(log);

            await this.db.SaveChangesAsync();
        }
    }
}
