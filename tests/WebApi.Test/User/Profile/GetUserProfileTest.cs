using Common.TestUtilities.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest 
    {
        private readonly string METHOD = "user";

        private readonly string _name;
        private readonly string _email;
        private readonly Guid _userIdentifier;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success() 
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token: token);

        }
    }
}
