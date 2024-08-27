using Bogus;
using Common.TestUtilities.Cryptography;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.TestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build() {
            var passwordEncripter = PasswordEncripterBuilder.Build();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(user => user.UserId, f => Guid.NewGuid())
                .RuleFor(user => user.Name, f => f.Person.FullName)
                .RuleFor(user => user.Email, f => f.Internet.Email())
                .RuleFor(user => user.Password, f => passwordEncripter.Encrypt(password));

            return (user, password);
        }
    }
}
