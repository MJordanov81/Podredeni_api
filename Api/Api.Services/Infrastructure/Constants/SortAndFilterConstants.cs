using System.Collections.Generic;

namespace Api.Services.Infrastructure.Constants
{
    public static class SortAndFilterConstants
    {
        //Product filterable/sortable properties
        public const string Name = "name";

        public const string Number = "number";

        public const string Quantity = "quantity";

        public const string Price = "price";

        public const string Category = "category";

        public const string Subcategory = "subcategory";

        public const string IsBlocked = "isblocked";

        public const string IsTopSeller = "istopseller";

        public const string LastModificationDate = "lastmodificationdate";

        public const string Status = "status";

        public static readonly Dictionary<string, int> OrderStatus = new Dictionary<string, int>
        {
            { "ordered", 0},
            { "confirmed", 1},
            { "dispatched", 2},
            { "cancelled", 3}
        };
    }
}
