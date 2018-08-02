namespace Api.Domain.Entities
{
    using System;

    public class OrderLog
    {
        public string Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Action { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
