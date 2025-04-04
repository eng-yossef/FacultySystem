using Microsoft.AspNetCore.Mvc;
using FacultySystem.Filters;


namespace FacultySystem.Controllers
{
    public class FilterController : Controller
    {
        [HandleError]
        public IActionResult Index()
        {
            throw new Exception("This is a test exception");

        }
    }
}
