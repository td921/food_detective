namespace food_detective.Models
{
    public class Food
    {
        public string Description { get; set; }
        public string? BrandOwner { get; set; }
        public string Ingredients { get; set; }
        public bool IsSelected { get; set; }
    }

    public class FoodArrayViewModel
    {
        public Food[] Foods { get; set; }
    }
}
