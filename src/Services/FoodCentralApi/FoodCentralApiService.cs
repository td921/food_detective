using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using Newtonsoft.Json;

namespace food_detective.Services.FoodCentralApi
{
    public class FoodCentralApiService : IFoodCentralApiService
    {
        public async Task<FoodsControllerApiResponse> FetchFoods(string foodName, string? brand, FoodSearchRequestBody requestBody)
        {
            string jsonResponse = "";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.nal.usda.gov/fdc/v1/foods/search");
                string queryString = $"?query={foodName}&brandOwner={brand}&pageSize={requestBody.pageSize}&pageNumber={requestBody.pageNumber}&sortBy=dataType.keyword&sortOrder=asc&api_key=DEMO_KEY";
                string requestUrl = client.BaseAddress + queryString;
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                jsonResponse = await response.Content.ReadAsStringAsync();
            }

            var foods = JsonConvert.DeserializeObject<FoodsControllerApiResponse>(jsonResponse);

            return foods;
        }
    }
}
