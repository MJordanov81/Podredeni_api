namespace Api.Models.Infrastructure.Constants
{
    //125

    public class ModelConstants
    {
        #region "Account"

        public const string EmailRequired = "Email is required";
        public const string PasswordRequired = "Password is required";

        public const int PasswordLength = 4;
        public const string PasswordRequirementsError = "Password must be at least 4 characters long";

        #endregion

        #region "Delivery data"

        public const int CustomerNameLength = 100;
        public const string CustomerNameLengthError = "Customer name must be less than 100 characters long";

        public const int PhoneNumberLength = 100;
        public const string PhoneNumberLengthError = "Phone number must be less than 100 characters long";

        public const int EmailLength = 100;
        public const string EmailLengthError = "Email must be less than 100 characters long";

        public const int CountryLength = 100;
        public const string CountryLengthError = "Country must be less than 100 characters long";

        public const int CityLength = 100;
        public const string CityLengthError = "City must be less than 100 characters long";

        public const int StreetLength = 100;
        public const string StreetLengthError = "Street must be less than 100 characters long";

        public const int PostCodeLength = 4;
        public const string PostCodeLengthError = "Post code must be less than 4 characters long";

        public const int StreetNumberLength = 10;
        public const string StreetNumberLengthError = "Street number must be less than 10 characters long";

        public const int DistrictLength = 100;
        public const string DistrictLengthError = "District must be less than 100 characters long";

        public const int BlockLength = 10;
        public const string BlockLengthError = "Block number must be less than 10 characters long";

        public const int EntranceLength = 10;
        public const string EntranceLengthError = "Entrance number must be less than 10 characters long";

        public const int FloorLength = 10;
        public const string FloorLengthError = "Floor number must be less than 10 characters long";

        public const int ApartmentLength = 10;
        public const string ApartmentLengthError = "Apartment number must be less than 10 characters long";

        public const int CommentsLength = 2500;
        public const string CommentsLengthError = "Comments must be less than 2500 characters long";

        public const int OfficeAddressLength = 200;
        public const string OfficeAddressLengthError = "Office address must be less than 200 characters long";

        public const int OfficeCityLength = 200;
        public const string OfficeCityLengthError = "Office city must be less than 200 characters long";

        public const int OfficeNameLength = 200;
        public const string OfficeNameLengthError = "Office name must be less than 150 characters long";

        public const int OfficeCountryLength = 200;
        public const string OfficeCountryLengthError = "Office country must be less than 50 characters long";

        #endregion

        #region "Product"

        public const int NameLength = 100;
        public const string NameLengthError = "Name must be no more than 100 characters long";

        public const int DescriptionLength = 1000;
        public const string DescriptionLengthError = "Description must be no more than 1000 characters long";

        public const string PriceRangeError = "Price cannot be negative";

        #endregion

        #region "Order"

        public const string ProductsEmptyArrayError = "There should be at least one product in order to create an order";

        #endregion

        #region "Category"

        public const string InvalidCategoryName = "Name cannot be an empty string";

        #endregion

        #region "News"

        public const int NewsTitleMinLength = 3;
        public const int NewsTitleMaxLength = 150;
        public const string NewsTitleLengthError = "News title must be between 3 and 150 characters long";

        public const int NewsImageUrlMinLength = 3;
        public const int NewsImageUrlMaxLength = 500;
        public const string NewsImageUrlLengthError = "News imageUrl must be between 3 and 500 characters long";

        public const int NewsContentMinLength = 3;
        public const int NewsContentMaxLength = 5000;
        public const string NewsContentLengthError = "News content must be between 3 and 5000 characters long";

        #endregion

        #region "Partner"

        public const int PartnerNameMaxLength = 150;
        public const string PartnerNameLengthError = "Partner name length cannot exceed 150 characters";

        public const int PartnerCategoryMaxLength = 20;
        public const string PartnerCategoryLengthError = "Partner category length cannot exceed 20 characters";

        #endregion

        #region "PartnerLocation"

        public const int PartnerLocationCityMaxLength = 50;
        public const string PartnerLocationCityLengthError = "Partner location city length cannot exceed 50 characters";

        public const int PartnerLocationAddressMaxLength = 300;
        public const string PartnerLocationAddressLengthError = "Partner location address length cannot exceed 300 characters";

        #endregion

        #region "Promotion"

        public const int PromotionNameMinLength = 3;
        public const int PromotionNameMaxLength = 150;
        public const string PromotionNameLengthError = "Promotion name must be between 3 and 150 characters long";

        public const int PromotionPromoCodeMinLength = 6;
        public const int PromotionPromoCodeMaxLength = 20;
        public const string PromotionPromoCodeLengthError = "Promotion promocode must be between 6 and 20 characters long";

        public const string PromotionProductsCountError = "Products must be greater than 1";

        public const string PromotionDiscountedProductsCountError = "Discounted products must be greater than 1";

        public const string PromotionDiscountError = "Dicount must be a positive decimal number";

        public const string PromotionQuotaError = "Quota must be a positive integer";

        public const int PromotionDescriptionMaxLength = 1000;
        public const string PromotionDescriptionError = "Promotion description must be less than 1000 characters long";

        #endregion

        #region "PromoDiscount"

        public const int PromoDiscountDiscountMinValue = 0;
        public const int PromoDiscountDiscountMaxValue = 100;
        public const string PromoDiscountDiscountError = "Discount must be between 0 and 100";

        public const int PromoDiscountNameMinLenght = 3;
        public const int PromoDiscountNameMaxLenght = 100;
        public const string PromoDiscountNameError = "Name must be between 3 and 100 characters long";

        #endregion

        #region "Video"

        public const int VideoDescriptionLength = 200;
        public const string VideoDescriptionLengthError = "Video description length cannot exceed 200 characters";

        public const int VideoUrlLength = 2000;
        public const string VideoUrlLengthError = "Video Url length cannot exceed 2000 characters";

        #endregion

        public const string AnonymousUser = "Anonymous user";
    }
}
