namespace Api.Services.Interfaces
{
    using Api.Models.Account;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<UserTokenModel> Register(string email, string password);

        Task<UserTokenModel> Login(string email, string password);

        Task SeedAdmin(string email, string password, string roleName);

        bool CheckRole(string userEmail, string roleName);

        string GetUserId(string userEmail);
    }
}
