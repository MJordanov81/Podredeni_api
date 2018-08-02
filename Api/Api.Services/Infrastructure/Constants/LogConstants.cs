namespace Api.Services.Infrastructure.Constants
{
    public static class LogConstants
    {
        public const string OrderCreated = "Поръчката е създадена";

        public const string OrderChanged = "Поръчката е променена";

        public const string OrderConfirmed = "Поръчката е потвърдена";

        public const string OrderReset = "Статусът на поръчката е отменен";

        public const string OrderDispatched = "Поръчката е изпратена";

        public const string OrderCancelled = "Поръчката е отказана";

        public const string StatusChanged = "Статусът е промемен от {0} на {1}";
    }
}
