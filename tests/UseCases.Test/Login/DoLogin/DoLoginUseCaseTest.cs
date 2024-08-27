using Common.TestUtilities.Cryptography;
using Common.TestUtilities.Entities;
using Common.TestUtilities.Repositories;
using Common.TestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        private static DoLoginUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
        {
            var encriptedPassword = PasswordEncripterBuilder.Build();
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

            if (user is not null)
                userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);

            return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), encriptedPassword);
        }

        [Fact]
        public async Task Success()
        {
            //Arrange 
            (var user, var password) = UserBuilder.Build();
            
            var useCase = CreateUseCase(user);

            //Act
            var result = await useCase.Execute(new RequestLoginJson
            {
                Email = user.Email,
                Password = password
            });

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
        }


        [Fact]
        public async Task Error_Invalid_User()
        {
            //Arrange 
            var request = RequestLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            //Act
            Func<Task> act = async() => await useCase.Execute(request);

            //assert
            await act.Should().ThrowAsync<InvalidLoginException>()
                .Where(e => e.Message.Equals(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID));

        }
    }
}
