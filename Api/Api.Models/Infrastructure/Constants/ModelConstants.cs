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

        public const int PartnerDetailsMaxLength = 4000;
        public const string PartnerDetailsLengthError = "Partner details length cannot exceed 4000 characters";

        #endregion

        public const string AnonymousUser = "Anonymous user";
    }
}
