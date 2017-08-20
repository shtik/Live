using Microsoft.AspNetCore.Mvc;

namespace ShtikLive.Shows.Controllers
{
    public static class ResultMethods
    {
        public static IActionResult Ok(object value)
        {
            return new OkObjectResult(value);
        }

        public static IActionResult NotFound()
        {
            return new NotFoundResult();
        }

        public static IActionResult CreatedAtAction(string actionName, string controllerName, object routeValues,
            object value)
        {
            return new CreatedAtActionResult(actionName, controllerName, routeValues, value);
        }
    }
}