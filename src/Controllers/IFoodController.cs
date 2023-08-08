using food_detective.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace food_detective.Controllers
{
    public interface IFoodController
    {
        Task<IActionResult> GetFoods([FromQuery] FoodSearchRequestBody foodSearchRequestBody);
    }
}
