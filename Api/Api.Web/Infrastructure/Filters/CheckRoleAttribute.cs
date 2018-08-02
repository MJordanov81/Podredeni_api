namespace Api.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Security.Claims;

    public class CheckRoleAttribute : ActionFilterAttribute
    {
        public CheckRoleAttribute(string role)
        {
            this.Role = role;
        }

        public string Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ClaimsPrincipal user = new ClaimsPrincipal();


            string email = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
