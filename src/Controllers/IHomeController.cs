using Microsoft.AspNetCore.Mvc;

namespace food_detective.Controllers
{
    public interface IHomeController
    {
        public ActionResult ToggleDarkMode();
    }
}
