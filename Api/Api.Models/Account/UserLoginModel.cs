namespace Api.Models.Account
{
    using Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class UserLoginModel
    {
        [Required(ErrorMessage = ModelConstants.EmailRequired)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = ModelConstants.PasswordRequired)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
