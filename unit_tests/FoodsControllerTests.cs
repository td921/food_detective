using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using food_detective.Controllers;
using Microsoft.AspNetCore.Mvc;
using food_detective.Models.RequestModels;
using food_detective.Models.ResponseModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;
using Microsoft.Owin.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace unit_tests

{
    [TestFixture]
    public class FoodsControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private WebApplication _app;
        private TestServer _server;
        private HttpClient _client;
        private FoodsController _foodsController;

        [SetUp]
        public void Setup()
        {
            // Create the WebApplicationFactory with the Startup class and specify the desired port
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                // Set the desired port here
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