﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using food_detective.Security;

namespace food_detective.Controllers
{
    public class FoodsController : Controller
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

            if (foodName == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "Food name cannot be empty."
                };

                return View("Error", errorViewModel);
            }

            var stringVerificationPass = StringExtensions.StringVerification(foodName, brand);

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
                string queryString = $"?query={foodName}&brandOwner={brand}&pageSize={foodSearchRequestBody.pageSize}&pageNumber={foodSearchRequestBody.pageNumber}&sortBy=dataType.keyword&sortOrder=asc&api_key=DEMO_KEY";
                string requestUrl = client.BaseAddress + queryString;

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var foods = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                if (foods.Foods.Length == 0)
                {
                    var errorViewModel = new ErrorViewModel
                    {
                        Message = "Sorry, we couldn't find any foods that match the food name you provided. Please try again."
                    };

                    return View("Error", errorViewModel);
                }

                var foodArrayViewModel = new FoodArrayViewModel
                {
                    Foods = foods.Foods
                };

                return View(foodArrayViewModel);
            }
        }
    }
}
