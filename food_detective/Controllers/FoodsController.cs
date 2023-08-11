using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using Microsoft.Extensions.Logging;

namespace food_detective.Controllers
{
    public class FoodsController : Controller
    {
        private readonly ILogger<FoodsController> _logger;

        public FoodsController(ILogger<FoodsController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery] FoodSearchRequestBody foodSearchRequestBody)
        {
            if (foodSearchRequestBody.apiKey == null)
            {
                ViewBag.ErrorMessage = "No API key was supplied. Get one at https://api.data.gov";
            }

            foodSearchRequestBody.pageSize = 200;
            foodSearchRequestBody.pageNumber = 1;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/foods/search");
                string queryString = $"?query={foodSearchRequestBody.query}&pageSize={foodSearchRequestBody.pageSize}&pageNumber={foodSearchRequestBody.pageNumber}&sortBy=dataType.keyword&sortOrder=asc&api_key={foodSearchRequestBody.apiKey}";
                string requestUrl = client.BaseAddress + queryString;

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var foods = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
                var foodArrayViewModel = new FoodArrayViewModel
                {
                    Foods = foods.Foods
                };

                return View(foodArrayViewModel);
            }
        }
    }
}
