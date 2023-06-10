namespace food_detective.Models.RequestModels
{
    public class FoodSearchRequestBody
    {
        public string query { get; set; }
        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string? brandOwner { get; set; }
    }
}
