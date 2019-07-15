namespace Api.Web.Controllers
{
    using Api.Models.Settings;
    using Api.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public abstract class BaseController : Controller
    {
        private readonly IUserService users;

        private readonly ISettingsService settings;

        protected BaseController(IUserService users, ISettingsService settings)
        {
            this.users = users;
            this.settings = settings;
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

        //Executes specific operations in controller actions provided as delegates and return IActionResult
        protected async Task<IActionResult> Execute(bool isAdmin, bool checkState, Func<Task<IActionResult>> function)
        {
            if (isAdmin && !this.IsInRole("admin"))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized);
            }

            if (checkState && !this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            //SettingsViewEditModel settings = await this.settings.Get();

            //this.HttpContext
            //    .Response
            //    .Headers
            //    .Add("Access-Control-Expose-Headers", "ApiSettings");

            //this.HttpContext
            //    .Response
            //    .Headers
            //    .Add("ApiSettings", JsonConvert.SerializeObject(settings.Settings));

            try
            {
                return await function();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, e.Message);
            }
        }
    }
}