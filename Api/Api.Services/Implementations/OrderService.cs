namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Domain.Enums;
    using Api.Models.Order;
    using Api.Models.ProductOrder;
    using Api.Models.Shared;
    using AutoMapper.QueryableExtensions;
    using Infrastructure.Constants;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly ApiDbContext db;

        private readonly IOrderLogService logger;

        private readonly INumeratorService numerator;

        private string modification;

        public OrderService(ApiDbContext db, INumeratorService numerator, IOrderLogService logger)
        {
            this.db = db;
            this.numerator = numerator;
            this.logger = logger;
        }

        public async Task<string> Create(OrderWithoutUserCreateModel data, string userId)
        {
            modification = LogConstants.OrderCreated;

            if (data.Products.Count < 1)
            {
                throw new ArgumentException(ErrorMessages.InvalidProducts);
            }

            if (!this.db.DeliveryData.Any(d => d.Id == data.DeliveryDataId))
            {
                throw new ArgumentException(ErrorMessages.InvalidOrderId);
            }

            if (data.PromoCode != null)
            {
                await this.UpdatePromotionUsedQuota(data.PromoCode, data.PromotionsCount);
            }

            int number = await this.numerator.GetNextNumer(typeof(Order));

            Order order = new Order
            {
                Number = number,
                LastModificationDate = DateTime.Now,
                UserId = userId,
                DeliveryDataId = data.DeliveryDataId,
                InvoiceDataId = data.InvoiceDataId,
                Status = OrderStatus.Ordered
            };

            await this.db.Orders.AddAsync(order);

            await this.db.SaveChangesAsync();

            string orderId = order.Id;

            await this.CreateProductOrders(order.Id, data.Products);

            await this.logger.Log(order.Id, userId, modification);

            return orderId;
        }

        private async Task UpdatePromotionUsedQuota(string promoCode, int promotionsCount)
        {
            Promotion promo = await this.db.Promotions.FirstOrDefaultAsync(p => p.PromoCode == promoCode);

            if (promo != null)
            {
                promo.UsedQuota += promotionsCount > 0 ? promotionsCount : 1;

                await this.db.SaveChangesAsync();
            }
        }

        public async Task<string> Edit(string orderId, string userId, OrderWithoutUserEditModel data)
        {
            if (data.Products.Count < 1)
            {
                throw new ArgumentException(ErrorMessages.InvalidProducts);
            }

            if (!this.db.Orders.Any(o => o.Id == orderId))
            {
                throw new ArgumentException(ErrorMessages.InvalidOrderId);
            }

            Order order = await this.db.Orders.FindAsync(orderId);

            order.LastModificationDate = DateTime.Now;

            await this.db.SaveChangesAsync();

            string modification = LogConstants.OrderChanged;

            await this.UpdateProductOrders(orderId, data.Products);

            await this.logger.Log(order.Id, userId, modification);

            return orderId;
        }

        public async Task<OrderDetailsModel> Get(string orderId)
        {
            if (!this.db.Orders.Any(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            return this.db.Orders
                .Where(p => p.Id == orderId)
                .ProjectTo<OrderDetailsModel>()
                .FirstOrDefault();
        }

        public async Task<string> GetEmail(string orderId)
        {
            if (!await this.db.Orders.AnyAsync(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            return this.db.Orders.FindAsync(orderId).Result.DeliveryData.CustomerData.Email;
        }

        public async Task SetConfirmationMailSent(string orderId)
        {
            if (!this.db.Orders.Any(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            Order order = this.db.Orders.FirstOrDefault(o => o.Id == orderId);

            order.IsConfirmationMailSent = true;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> IsConfirmationMailSent(string orderId)
        {
            if (!this.db.Orders.Any(o => o.Id == orderId)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            return await this.db.Orders
                .Where(o => o.Id == orderId)
                .Select(o => o.IsConfirmationMailSent)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderDetailsListPaginatedModel> GetAll(PaginationModel pagination)
        {
            IEnumerable<OrderDetailsModel> orders = db.Orders
                .OrderByDescending(o => o.LastModificationDate)
                .ProjectTo<OrderDetailsModel>()
                .ToList();

            if (!string.IsNullOrEmpty(pagination.FilterElement))
            {
                orders = await this.FilterElements(orders, pagination.FilterElement, pagination.FilterValue);
            }

            if (!string.IsNullOrEmpty(pagination.SortElement))
            {
                orders = await this.SortElements(orders, pagination.SortElement, pagination.SortDesc);
            }

            int ordersCount = orders.Count();

            if (pagination.Page < 1) pagination.Page = 1;

            if (pagination.Size < 1) pagination.Size = 5;

            orders = orders.Skip(pagination.Size * (pagination.Page - 1)).Take(pagination.Size).ToList();

            return new OrderDetailsListPaginatedModel
            {
                Orders = orders,
                OrdersCount = ordersCount
            };
        }

        public async Task<OrderDetailsModel> ChangeStatus(string id, string userId, OrderStatus newStatus)
        {
            if (!this.db.Orders.Any(o => o.Id == id)) throw new ArgumentException(ErrorMessages.InvalidOrderId);

            Order order = this.db.Orders.FirstOrDefault(o => o.Id == id);

            order.LastModificationDate = DateTime.Now;

            order.Status = newStatus;

            string action = "";

            switch (newStatus)
            {
                case OrderStatus.Confirmed:
                    action = LogConstants.OrderConfirmed;
                    break;
                case OrderStatus.Dispatched:
                    action = LogConstants.OrderDispatched;
                    break;
                case OrderStatus.Cancelled:
                    action = LogConstants.OrderCancelled;
                    break;
                default:
                    action = LogConstants.OrderReset;
                    break;
            }

            await logger.Log(id, userId, action);

            return this.db.Orders
                .Where(o => o.Id == id)
                .ProjectTo<OrderDetailsModel>()
                .FirstOrDefault();
        }

        #region "UpdateProductOrders"

        private async Task UpdateProductOrders(string orderId, ICollection<ProductOrderCreateModel> products)
        {
            List<ProductOrder> productOrders = this.db.ProductOrders.Where(po => po.OrderId == orderId).ToList();

            this.db.RemoveRange(productOrders);

            await this.db.SaveChangesAsync();

            await this.CreateProductOrders(orderId, products);
        }

        private async Task CreateProductOrders(string orderId, ICollection<ProductOrderCreateModel> products)
        {
            foreach (var product in products)
            {
                ProductOrder po = new ProductOrder
                {
                    OrderId = orderId,
                    ProductId = product.Id,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Discount = product.Discount
                };

                await this.db.ProductOrders.AddAsync(po);

                await this.db.SaveChangesAsync();
            }

        }

        #endregion

        #region "FilterAndSort"

        private async Task<IEnumerable<OrderDetailsModel>> SortElements(IEnumerable<OrderDetailsModel> orders, string sortElement, bool sortDesc)
        {
            sortElement = sortElement.ToLower();

            if (sortElement == SortAndFilterConstants.Number)
            {
                if (sortDesc)
                {
                    return orders.OrderByDescending(o => o.Number).ToArray();
                }
                else
                {
                    return orders.OrderBy(o => o.Number).ToArray();
                }
            }

            if (sortElement == SortAndFilterConstants.Status)
            {

                if (sortDesc)
                {
                    return orders.OrderByDescending(o => o.Status).ToArray();
                }
                else
                {
                    return orders.OrderBy(o => o.Status).ToArray();
                }
            }

            if (sortElement == SortAndFilterConstants.LastModificationDate)
            {
                if (sortDesc)
                {
                    return orders.OrderByDescending(o => o.LastModificationDate).ToArray();
                }
                else
                {
                    return orders.OrderBy(o => o.LastModificationDate).ToArray();
                }
            }

            return orders;
        }

        private async Task<IEnumerable<OrderDetailsModel>> FilterElements(IEnumerable<OrderDetailsModel> orders, string filterElement, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                filterElement = filterElement.ToLower();
                filterValue = filterValue.ToLower();

                if (filterElement == SortAndFilterConstants.Number)
                {
                    return orders.Where(o => o.Number.ToString().Contains(filterValue));
                }

                else if (filterElement == SortAndFilterConstants.Status)
                {
                    filterValue = filterValue.Substring(0, 1).ToUpper() + filterValue.Substring(1);

                    OrderStatus status = (OrderStatus)Enum.Parse(typeof(OrderStatus), filterValue);

                    return orders.Where(o => o.Status == status);
                }

                else if (filterElement == SortAndFilterConstants.LastModificationDate)
                {
                    DateModel date = this.ParseDate(filterValue);

                    return orders.Where(o => o.LastModificationDate == new DateTime(date.Year, date.Month, date.Day));
                }
            }

            return orders;
        }

        #endregion

        #region "Utils"

        private DateModel ParseDate(string filterValue)
        {
            int[] tokens = filterValue.Split('.').Select(int.Parse).ToArray();

            return new DateModel
            {
                Year = tokens[0],
                Month = tokens[1],
                Day = tokens[2]
            };
        }

        #endregion
    }
}
