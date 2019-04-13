namespace Api.Models.Order
{
    using Api.Common.Mapping;
    using Api.Domain.Entities;
    using Api.Domain.Enums;
    using AutoMapper;
    using Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OrderDetailsModel : IMapFrom<Order>, IHaveCustomMapping
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public DateTime LastModificationDate { get; set; }

        public OrderStatus Status { get; set; }

        bool IsConfirmationMailSent { get; set; }

        public ICollection<ProductInOrderDetailsModel> Products { get; set; }

        public string UserId { get; set; }

        public string DeliveryDataId { get; set; }

        public string InvoiceDataId { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Order, OrderDetailsModel>()
                .ForMember(om => om.Products, cfg => cfg.MapFrom(o => o.ProductOrders.Select(po =>
                new ProductInOrderDetailsModel
                {
                    Id = po.ProductId,
                    ProductOrderId = po.Id,
                    Name = po.Product.Name,
                    Image = po.Product.Images.Select(i => i.Url).FirstOrDefault(),
                    Price = po.Price,
                    Quantity = po.Quantity,
                    Discount = po.Discount
                })));
        }
    }
}

