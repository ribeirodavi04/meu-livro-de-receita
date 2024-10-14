using Common.TestUtilities.Entities;
using Common.TestUtilities.LoggedUser;
using Common.TestUtilities.Mapper;
using Common.TestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {

        [Fact]
        public async Task Success()
        {
            //Arrange
            (var user, var _) = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act
            var result = await useCase.Execute();

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
        }

        private static GetUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }

    }
}
