namespace Api.Web.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface ITokenService
    {
        Task<string> ProvideToken(IEnumerable<Claim> claims);
    }
}
