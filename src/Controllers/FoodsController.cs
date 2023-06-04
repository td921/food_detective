using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using System.Text.RegularExpressions;
using Microsoft.Extensions.ObjectPool;
using System.Net;
using food_detective.Security;

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
            var foodName = foodSearchRequestBody.query;
            var brand = foodSearchRequestBody.brandOwner;

            if (foodSearchRequestBody.apiKey == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "No API key was supplied. Get one at https://api.data.gov"
                };

                return View("Error", errorViewModel);
            }

            if (foodSearchRequestBody.query == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Food name cannot be empty."
                };

                return View("Error", errorViewModel);
            }

            var stringVerificationPass = StringVerification(foodName, brand);

            if (!stringVerificationPass)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Sorry, the input provided contains unaccepted content. Please review your input and try again."
                };

                return View("Error", errorViewModel);
            }

            foodSearchRequestBody.pageSize = 100;
            foodSearchRequestBody.pageNumber = 1;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/foods/search");
                string queryString = $"?query={foodName}&brandOwner={brand}&pageSize={foodSearchRequestBody.pageSize}&pageNumber={foodSearchRequestBody.pageNumber}&sortBy=dataType.keyword&sortOrder=asc&api_key={foodSearchRequestBody.apiKey}";
                string requestUrl = client.BaseAddress + queryString;

                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    var errorViewModel = new ErrorViewModel
                    {
                        Message = "Sorry, the pass key provided is invalid. Sign up for a pass key at<br>https://fdc.nal.usda.gov/api-key-signup.html"
                    };

                    return View("Error", errorViewModel);
                }    
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var foods = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);
                var foodArrayViewModel = new FoodArrayViewModel
                {
                    Foods = foods.Foods
                };

                return View(foodArrayViewModel);
            }
        }

        private bool StringVerification(string foodName, string? brand)
        {
            foodName = Regex.Replace(foodName, StringExtensions.StripHtml, string.Empty);
            foodName = Regex.Replace(foodName, StringExtensions.StripUrl, string.Empty);

            if (!string.IsNullOrEmpty(brand))
            {
                brand = Regex.Replace(brand, StringExtensions.StripHtml, string.Empty);
                brand = Regex.Replace(brand, StringExtensions.StripUrl, string.Empty);

                if (string.IsNullOrEmpty(brand))
                {
                    return false;
                }
            }

            if (string.IsNullOrEmpty(foodName))
            {
                return false;
            }

            return true;
        }
    }
}
