using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;

namespace food_detective.Services.FoodCentralApi
{
    public interface IFoodCentralApiService
    {
        Task<FoodsControllerApiResponse> FetchFoods(string foodName, string? brand, FoodSearchRequestBody requestBody);
    }
}
