namespace Api.Models.Order
{
    using System.Collections.Generic;

    public class OrderDetailsListPaginatedModel
    {
        public IEnumerable<OrderDetailsModel> Orders { get; set; }

        public int OrdersCount { get; set; }
    }
}
