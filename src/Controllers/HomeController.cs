using Microsoft.AspNetCore.Mvc;

namespace food_detective.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost("home/toggledarkmode")]
        public ActionResult ToggleDarkMode()
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMonths(3);
            options.Path = "/";

            HttpContext.Response.Cookies.Append("DarkMode", "true", options);

            return View("Index");
        }
    }
}
