using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DeveSpotnet.Controllers.NewzNabApiControllerHelpers
{
    public class BooleanBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Parse "1" or "0" as boolean
            if (value == "1")
            {
                bindingContext.Result = ModelBindingResult.Success(true);
            }
            else if (value == "0")
            {
                bindingContext.Result = ModelBindingResult.Success(false);
            }
            else if (bool.TryParse(value, out bool result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid boolean value.");
            }

            return Task.CompletedTask;
        }
    }
}
