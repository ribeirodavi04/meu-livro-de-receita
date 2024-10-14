using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test
{
    public class MyRecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public MyRecipeBookClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost(string method, object request)
        {
            return await _httpClient.PostAsJsonAsync(method, request);
        }

        protected async Task<HttpResponseMessage> DoGet(string method, string token = "")
        {
            
        }
    }
}
