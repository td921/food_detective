using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using Microsoft.Extensions.Logging;

namespace food_detective.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly ILogger<IngredientsController> _logger;

        public IngredientsController(ILogger<IngredientsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredients(string ingredients)
        {
                return View("GetAllIngredients", ingredients);
            }
        }

        /*[HttpGet("~/getharmfulingredients")]
        public async Task<IActionResult> GetHarmfulIngredients(int id, string apiKey)
        {
            if (apiKey == null)
            {
                return StatusCode(403, new { Message = "No API key was supplied. Get one at https://api.data.gov" });

            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/food/");
                string queryString = $"{id}?api_key={apiKey}";
                string requestUrl = client.BaseAddress + queryString;

                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Food? food = JsonConvert.DeserializeObject<Food>(jsonResponse);

                    HarmfulIngredients harmfulIngredients = new HarmfulIngredients();

                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                    var ingredients = food.Ingredients.ToLower().Split(',').Select(i => textInfo.ToTitleCase(i.Trim()));

                    IEnumerable<string> matchedIngredients = ingredients.Intersect(harmfulIngredients.ingredients).ToArray();

                    return Ok(matchedIngredients);
                }
                else
                {
                    return BadRequest(new { Message = $"API request failed because {jsonResponse}" });
                }
            }
        }
    } */
}
