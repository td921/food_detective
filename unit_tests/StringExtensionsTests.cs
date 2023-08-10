using food_detective.Security;

namespace unit_tests
{
    internal class StringExtensionsTests
    {
        [TestCase("www. duckduckgo. com", "https://www.duckduckgo.com")]
        [TestCase("www. duckduckgo. com", "http://www.duckduckgo.com")]
        [TestCase("duckduckgo. com/home", "duckduckgo.com/home")]
        [TestCase("Ratatouillewww. duckduckgo. com", "Ratatouillehttps://www.duckduckgo.com")]
        [TestCase("Ratatouillewww. duckduckgo. com", "Ratatouillehttp://www.duckduckgo.com")]
        [TestCase("Ratatouille www. duckduckgo. com", "Ratatouille https://www.duckduckgo.com")]
        [TestCase("Ratatouille www. duckduckgo. com", "Ratatouille http://www.duckduckgo.com")]
        [TestCase("Ratatouillewww. duckduckgo. com French Dish", "Ratatouillehttp://www.duckduckgo.com French Dish")]
        [TestCase("Wonton:www. duckduckgo. com", "Wonton:www.duckduckgo.com")]
        [TestCase("Wonton:www. duckduckgo. com", "Wonton:https://www.duckduckgo.com")]
        [TestCase("Wonton:www. duckduckgo. com", "Wonton:http://www.duckduckgo.com")]
        [TestCase("Wonton:www. duckduckgo. com Soup", "Wonton:http://www.duckduckgo.com Soup")]
        public void RemoveUrlShouldReplaceUrl(string expected, string input)
        {
            var result = input.RemoveUrl();

            Assert.AreEqual(expected, result);
        }

        [TestCase("Lucky Charms Cereal", "Lucky Charms Cereal")]
        [TestCase("Lucky Charms Cereal", "Lucky Charms Cereal<a href='https://www.duckduckgo.com'></a>")]
        [TestCase("Lucky Charms CerealMarshmallows", "Lucky Charms Cereal<a href='https://www.duckduckgo.com'></a>Marshmallows")]
        [TestCase("Lucky Charms CerealContainsMarshmallows", "Lucky Charms Cereal<a href='https://www.duckduckgo.com'>Contains</a>Marshmallows")]
        public void RemoveHtmlShouldReplaceHtml(string expected, string input)
        {
            var result = input.RemoveHtml();

            Assert.AreEqual(expected, result);
        }
    }
}
