namespace Api.Web.Controllers
{
    using Api.Data;
    using Api.Models.Account;
    using Api.Services.Interfaces;
    using Api.Web.Models.Config;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Produces("application/json")]
    public class AccountController : BaseController
    {
        private readonly ITokenService jwtTokenService;
        private readonly ApiDbContext db;
        private readonly IUserService users;
        private readonly AdminCredentials adminCredentials;

        public AccountController(ITokenService jwtTokenService, ApiDbContext db, IUserService users, AdminCredentials adminCredentials) : base(users)
        {
            this.jwtTokenService = jwtTokenService;
            this.db = db;
            this.users = users;
            this.adminCredentials = adminCredentials;
        }

        //get api/
        [HttpGet]
        [Route("api/")]
        public async Task<IActionResult> Hello()
        {
            return this.Ok("Hello from Api");
        }

        //post api/users/login
        [HttpPost]
        [Route("api/users/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel inputUser)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            UserTokenModel user;

            if (inputUser.Email == adminCredentials.Email)
            {
                await this.users.SeedAdmin(adminCredentials.Email, adminCredentials.Password, adminCredentials.Role);
            }

            try
            {
                user = await this.users.Login(inputUser.Email, inputUser.Password);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            user.Token = await this.GetJwtToken(inputUser.Email);

            return this.Ok(user);
        }

        //post api/users/register
        [HttpPost]
        [Route("api/users/register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterModel inputUser)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            UserTokenModel user;

            try
            {
                user = await this.users.Register(inputUser.Email, inputUser.Password);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, e.Message);
            }

            user.Token = await this.GetJwtToken(inputUser.Email);

            return this.Ok(user);

        }

        # region "Utilities"

        private async Task<string> GetJwtToken(string userEmail)
        {
            Claim[] claims = new[]
{
                    new Claim(ClaimTypes.Name, userEmail)
                };

            return await this.jwtTokenService.ProvideToken(claims);
        }

        #endregion
    }
}
