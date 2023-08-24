using food_detective.Models.RequestModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace integration_tests
{
    internal class FoodsControllersTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private readonly WebApplication _app;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseUrls("http://localhost:7179");
            });

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:7179")
            });

        }

        public void TearDown()
        {
            // Stop the WebApplication and dispose the HttpClient
            _app?.StopAsync().GetAwaiter().GetResult();
            _client?.Dispose();
        }

        [Test]
        public async Task GetFoods_ReturnsOkWithValidData()
        {
            // Arrange
            var foodSearchRequestBody = new FoodSearchRequestBody
            {
                query = "Fruit Loops"
            };

            var queryString = $"?query={foodSearchRequestBody.query}";

            // Act
            var response = await _client.GetAsync("/Foods/GetFoods" + queryString);
            var responseBody = await response.Content.ReadAsStringAsync();

            int occurrences = CountOccurrences(responseBody);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.AreEqual(100, occurrences);
        }

        [Test]
        public async Task GetFoods_ReturnsErrorWhenNoInputProvided()
        {
            // Arrange
            var foodSearchRequestBody = new FoodSearchRequestBody
            {
            };

            var queryString = $"?query={foodSearchRequestBody.query}";

            // Act
            var response = await _client.GetAsync("/Foods/GetFoods" + queryString);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(responseBody.Contains("error-message") && responseBody.Contains("Food name cannot be empty."));
        }

        [Test]
        public async Task GetFoods_ReturnsErrorWhenNoMatchFound()
        {
            // Arrange
            var foodSearchRequestBody = new FoodSearchRequestBody
            {
                query = "Parallellineshavesomuchincommon.It'sashamethey'llnevermeet."
            };

            var queryString = $"?query={foodSearchRequestBody.query}";

            // Act
            var response = await _client.GetAsync("/Foods/GetFoods" + queryString);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(responseBody.Contains("error-message") && responseBody.Contains("Sorry, we couldn't find any foods that match the food name you provided. Please try again."));
        }

        private int CountOccurrences(string text)
        {
            int count = 0, index = 0;
            var pattern = "name=\"ingredients\" value=\"";

            while ((index = text.IndexOf(pattern, index)) != -1)
            {
                count++;
                index += pattern.Length;
            }

            return count;
        }
    }
}
