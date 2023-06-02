using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using food_detective.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

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
        public IActionResult GetHarmfulIngredients(string ingredients)
        {
            HarmfulIngredients harmfulIngredients = new HarmfulIngredients();
            var harmfulIngredientsDictionary = harmfulIngredients.HarmfulIngredientsDictionary;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            char[] charactersToRemove = { ',', '.' };
            var ingredientsLowerCase = ingredients.ToLower().Split(',').Select(i => textInfo.ToTitleCase(i.Trim().TrimEnd(charactersToRemove)));

            var matchedIngredients = new Dictionary<string, string>();

            foreach (var element in ingredientsLowerCase)
            {
                foreach (KeyValuePair<string, string> entry in harmfulIngredientsDictionary)
                {
                    string pattern = @"\b" + Regex.Escape(entry.Key) + @"\w*\b";
                    Match match = Regex.Match(element, pattern, RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        matchedIngredients.Add(element, entry.Value);
                    }
                }
            }

            var ingredientsViewModel = new IngredientsViewModel
            {
                AllIngredients = ingredients,
                HarmfulIngredients = matchedIngredients
            };

            return View(ingredientsViewModel); 
        }
    }
}
