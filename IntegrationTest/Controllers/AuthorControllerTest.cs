using Fingers10.EnterpriseArchitecture.API.IntegrationTest.Models;
using FluentAssertions;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fingers10.EnterpriseArchitecture.API.IntegrationTest.Controllers
{
    public class AuthorControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public AuthorControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("https://localhost:44339/api/authors");
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetAuthors_ReturnsExpectedResponse()
        {
            //var expected = new List<string> { "Accessories", "Bags", "Balls", "Clothing", "Rackets" };

            var model = await _client.GetFromJsonAsync<ExpectedAuthorsResponse>("");

            model.Should().NotBeNull();
            model.Value.First().Name.Should().Be("Abdul Rahman Shabeek Mohamed");
            //Assert.NotNull(model?.AllowedCategories);
            //Assert.Equal(expected.OrderBy(s => s), model.AllowedCategories.OrderBy(s => s));
        }

        //[Fact]
        //public async Task GetAll_SetsExpectedCacheControlHeader()
        //{
        //    var response = await _client.GetAsync("");

        //    var header = response.Headers.CacheControl;

        //    Assert.True(header.MaxAge.HasValue);
        //    Assert.Equal(TimeSpan.FromMinutes(5), header.MaxAge);
        //    Assert.True(header.Public);
        //}
    }
}
