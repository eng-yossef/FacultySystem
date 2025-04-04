using Microsoft.AspNetCore.Mvc.Filters;

namespace FacultySystem.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Before Action Execution");

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("After Action Execution");
        }
    }
}
