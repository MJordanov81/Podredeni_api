namespace Api.Domain.Entities
{
    using Api.Domain.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        public string Id { get; set; }

        [Range(0, int.MaxValue)]
        public int Number { get; set; }

        public DateTime LastModificationDate { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsConfirmationMailSent { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();

        [Required]
        public string DeliveryDataId { get; set; }

        public DeliveryData DeliveryData { get; set; }

        public string InvoiceDataId { get; set; }

        public InvoiceData InvoiceData { get; set; }

        public ICollection<OrderLog> OrderLogs { get; set; } = new List<OrderLog>();

    }
}
