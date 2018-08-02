namespace Api.Models.Infrastructure.Filters
{
    using Infrastructure.Constants;
    using System.ComponentModel.DataAnnotations;

    public class UserPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string password = value as string;

            if (password.Length < ModelConstants.PasswordLength) return false;

            return true;
        }
    }
}
