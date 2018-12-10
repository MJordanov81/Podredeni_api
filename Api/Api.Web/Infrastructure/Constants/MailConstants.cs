namespace Api.Web.Infrastructure.Constants
{
    public static class MailConstants
    {
        public const string SubjectConfirm = "Потвърждение за поръчка {0}";

        public const string SubjectCancel = "Отказ на поръчка {0}";

        public const string SubjectCreate = "Нова поръчка с номер {0}";

        public const string ContentConfirm = "Здравейте," + "\n" + "Вашата поръчка в Podredeni.eu беше потвърдена и ще бъде изпратена на посочения от Вас адрес/офис на Еконт." + "\n" + "Благодарим Ви, че пазарувахте от нас!";

        public const string ContentCancel = "Здравейте," + "\n" + "Вашата поръчка в Podredeni.eu беше отказана" + "\n" + "Заповядайте отново!";

        public const string ContactFormMailContent = "Имейл: {0}" + "\n" + "Име: {1}" + "\n" + "Съдържание: {2}";

        public const string OfficeMail = "office@podredeni.eu";

        public const string DebugMail = "marian.jordanov@gmail.com";
    }
}
