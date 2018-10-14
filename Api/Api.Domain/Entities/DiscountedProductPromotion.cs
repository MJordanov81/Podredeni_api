namespace Api.Domain.Entities
{
    public class DiscountedProductPromotion
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string PromotionId { get; set; }

        public Promotion Promotion { get; set; }
    }
}
