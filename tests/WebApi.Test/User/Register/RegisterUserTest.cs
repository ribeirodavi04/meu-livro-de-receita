using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.TestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly string method = "user";

        public RegisterUserTest(CustomWebApplicationFactory factory) 
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Success()
        {
            //arrange
            var request = RequestRegisterUserJsonBuilder.Build();

            //act
            var response = await _httpClient.PostAsJsonAsync("api/" + method, request);

            //assert
            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
        }
    }
}
