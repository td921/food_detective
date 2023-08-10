using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using food_detective.Security;

namespace food_detective.Controllers
{
    public class FoodsController : Controller, IFoodController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery] FoodSearchRequestBody foodSearchRequestBody)
        {
            var foodName = foodSearchRequestBody.query;
            var brand = foodSearchRequestBody.brandOwner;
            foodSearchRequestBody.pageSize = 100;
            foodSearchRequestBody.pageNumber = 1;

            var nullCheckResult = FoodNameNullCheck(foodName);

            if ((nullCheckResult as ViewResult).ViewName == "Error")
            {
                return nullCheckResult;
            }

            foodName = StringExtensions.RemoveHtml(foodName).RemoveUrl();
            brand = string.IsNullOrEmpty(brand) ? brand : StringExtensions.RemoveHtml(brand).RemoveUrl();

            if (string.IsNullOrEmpty(foodName))
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Sorry, the food name provided is invalid. Please review your input and try again."
                };

                return View("Error", errorViewModel);
            }

            var foods = await RetrieveFoodsFromApi(foodName, brand, foodSearchRequestBody);

            var foodsResponseLengthVerificationResult = FoodsResponseLengthCheck(foods.Foods.Length);

            if ((foodsResponseLengthVerificationResult as ViewResult).ViewName == "Error")
            {
                return foodsResponseLengthVerificationResult;
            }

            var foodArrayViewModel = new FoodArrayViewModel
            {
                Foods = foods.Foods
            };

            return View(foodArrayViewModel);
        }

        private async Task<FoodsControllerApiResponse> RetrieveFoodsFromApi(string foodName, string? brand, FoodSearchRequestBody foodSearchRequestBody)
        {
            string jsonResponse = "";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/foods/search");
                string queryString = $"?query={foodName}&brandOwner={brand}&pageSize={foodSearchRequestBody.pageSize}&pageNumber={foodSearchRequestBody.pageNumber}&sortBy=dataType.keyword&sortOrder=asc&api_key=DEMO_KEY";
                string requestUrl = client.BaseAddress + queryString;
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                jsonResponse = await response.Content.ReadAsStringAsync();
            }

            var foods = JsonConvert.DeserializeObject<FoodsControllerApiResponse>(jsonResponse);

            return foods;
        }

        private IActionResult FoodsResponseLengthCheck(int foodsResponseLength)
        {
            if (foodsResponseLength == 0)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Sorry, we couldn't find any foods that match the food name you provided. Please try again."
                };

                return View("Error", errorViewModel);
            }

            return View("SuccessfulFunctionCall");
        }

        private IActionResult FoodNameNullCheck(string foodName)
        {
            if (foodName == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Food name cannot be empty."
                };

                return View("Error", errorViewModel);
            }

            return View("SuccessfulFunctionCall");
        }
    }
}
