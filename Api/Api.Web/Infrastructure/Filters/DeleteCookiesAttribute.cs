namespace Api.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;

    public class DeleteCookiesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Remove("Cookies");

            base.OnActionExecuted(filterContext);
        }
    }
}
