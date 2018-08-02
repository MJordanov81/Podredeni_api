namespace Api.Models.Account
{
    using Infrastructure.Constants;
    using Infrastructure.Filters;
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterModel
    {
        [Required(ErrorMessage = ModelConstants.EmailRequired)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = ModelConstants.PasswordRequired)]
        [DataType(DataType.Password)]
        [UserPassword(ErrorMessage = ModelConstants.PasswordRequirementsError)]
        public string Password { get; set; }

        [Required(ErrorMessage = ModelConstants.PasswordRequired)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
