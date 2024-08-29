using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestUtilities.Tokens
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "5g22fRrLwbIn3HVq94HOqQYYbtsSBOwja");
    }
}
