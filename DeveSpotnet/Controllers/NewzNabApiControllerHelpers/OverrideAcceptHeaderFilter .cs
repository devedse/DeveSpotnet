using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace DeveSpotnet.Controllers.NewzNabApiControllerHelpers
{
    public class OverrideAcceptHeaderFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var format = request.Query.TryGetValue("o", out StringValues outputFormat) ? outputFormat.ToString().ToLower() : null;

            request.Headers["Accept"] = format == "json" ? "application/json" : "application/xml";
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
