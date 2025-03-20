using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace DeveSpotnet.Controllers.NewzNabApiControllerHelpers
{
    public class QueryParameterConstraintAttribute : Attribute, IActionConstraint
    {
        private readonly string _parameter;
        private readonly string _value;

        public QueryParameterConstraintAttribute(string parameter, string value)
        {
            _parameter = parameter;
            _value = value;
        }

        // Ensure this constraint runs early
        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var query = context.RouteContext.HttpContext.Request.Query;
            if (query.TryGetValue(_parameter, out var actualValue))
            {
                return string.Equals(actualValue.ToString(), _value, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
