namespace Api.Web.Controllers
{
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    public abstract class BaseController : Controller
    {
        private readonly IUserService users;

        protected BaseController(IUserService users)
        {
            this.users = users;
        }

        protected string UserId => this.GetCurrentUserId();

        protected bool IsInRole(string role)
        {
            string userEmail = this.GetCurrentUserEmail();

            if (userEmail == null) return false;

            return this.users.CheckRole(userEmail, role);
        }

        protected string GetCurrentUserId()
        {
            string userEmail = this.GetCurrentUserEmail();

            if (userEmail == null) return null;

            return this.users.GetUserId(userEmail);
        }

        protected string GetCurrentUserEmail()
        {
            return User.Identity?.Name;
        }
    }
}