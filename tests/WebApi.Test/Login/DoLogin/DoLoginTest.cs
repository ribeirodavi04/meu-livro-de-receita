using Common.TestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        private readonly string method = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();

            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }


        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            //Act
            var response = await _httpClient.PostAsJsonAsync("api/"+ method, request);

            //assert
            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Login_Invalid()
        {
            //Arrange
            var request = RequestLoginJsonBuilder.Build();

            //Act
            var response = await _httpClient.PostAsJsonAsync("api/" + method, request);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
            var expectedMessage = ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID.ToString();

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));

        }
    }
}
