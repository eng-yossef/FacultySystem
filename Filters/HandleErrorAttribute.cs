using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FacultySystem.Filters
{
    public class HandleErrorAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Log the exception
            var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<HandleErrorAttribute>)) as ILogger<HandleErrorAttribute>;
            logger?.LogError(context.Exception, "An error occurred in the application.");

            // Create a new ViewResult and set its properties
            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    context.ModelState)
                {
                    ["ErrorMessage"] = context.Exception.Message
                }
            };

            context.Result = result;
            // Mark the exception as handled
            context.ExceptionHandled = true;
        }
    }
}