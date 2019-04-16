namespace Api.Services.Infrastructure.Constants
{
    public static class ErrorMessages
    {
        //PRODUCT
        public const string InvalidUserCredentials = "Invalid email or password!";

        public const string EmailAlreadyRegistered = "A user with this email has already been registered";

        public const string UnableToWriteToDb = "Unable to write to database!";

        public const string InvalidImageUrl = "Image URL cannot be null or empty string";

        public const string InvalidType = "Type cannot be null";

        public const string InvalidProductParameters = "Name and/or descrition cannot be null and price cannot be negative";

        public const string InvalidProductId = "Product with the given id does not exist";

        public const string InvalidImageId = "Image with the given Id does not exist";

        public const string InvalidCategoryId = "Category with the given id does not exist";

        //DELIVERY DATA
        public const string InvalidOfficeAddress = "Office address cannot be empty";

        public const string InvalidCustomerData = "Customer name, phone number and email are required";

        //ORDER
        public const string InvalidProducts = "Cannot create an empty order";

        public const string InvalidOrderId = "Invalid order Id";

        //DELIVERY DATA
        public const string InvalidDeliveryDataId = "Invalid delivery data id";

        public const string InvalidUserId = "Invalid user id";

        public const string InvalidArguments = "Invalid arguments";

        //CATEGORY
        public const string InvalidCategoryName = "Category with the given name already exists";

        public const string InvalidCategoryDelete = "Unable to delete category (already in use?)";

        //SUBCATEGORY
        public const string InvalidSubcategoryName = "Subcategory with the given name already exists";

        //NEWS
        public const string InvalidNewsParameters = "News title, content and/or image url cannot be null or empty";

        public const string InvalidNewsId = "News with the given id does not exist";

        //VIDEO
        public const string InvalidVideoCreateData = "Video URL and description cannot be null or empty string";

        public const string InvalidVideoId = "Video with the given id does not exist";

        //PARTNER
        public const string InvalidPartnerCreateData = "Partner name and/or category cannot be null or empty";

        public const string InvalidPartnerId = "Partner with the given id does not exist";

        public const string InvalidPartnerLocationCreationData = "PartnerLocation city and/or address cannot be null or an empty strings";

        //PROMOTION
        public const string InvalidPromotionDates = "Start date cannot be later than end date";

        public const string InvalidPromotionProductsCount = "Please provide at least one product and one discounted product";

        public const string InvalidPromotionPromoCode = "Promo code already exists in DB, please provide a new one";

        public const string InvalidDeleteRequest = "Cannot delete this promotion because it has been already used";

        public const string InvalidPromotionId = "Promotion with the given id does not exist";
    }
}
