using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NutriFitApp.WebAdmin.Filters
{
    public class RequireTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tempDataFactory = context.HttpContext.RequestServices
                .GetService<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionaryFactory>();

            var tempData = tempDataFactory?.GetTempData(context.HttpContext);
            var token = tempData?["Token"] as string;

            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    ["controller"] = "Auth",
                    ["action"] = "Login"
                });
            }

            base.OnActionExecuting(context);
        }
    }
}
