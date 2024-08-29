using Common.TestUtilities.Cryptography;
using Common.TestUtilities.Mapper;
using Common.TestUtilities.Repositories;
using Common.TestUtilities.Requests;
using Common.TestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        private RegisterUserUseCase CreateUseCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var passwordEncripter = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (string.IsNullOrEmpty(email) == false)
                userReadOnlyRepository.ExistActiveUserWithEmail(email);

            return new RegisterUserUseCase(userReadOnlyRepository.Build(), userWriteOnlyRepository, unitOfWork, mapper, accessTokenGenerator, passwordEncripter);
        }

        [Fact]
        public async Task Success()
        {
            
            var useCase = CreateUseCase();

            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();

            //Act
            var result = await useCase.Execute(request);

            //Assert
            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            //Arrange
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUseCase(request.Email);

            //Act
            Func<Task> act = async () => await useCase.Execute(request);

            //Assert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorsMessages.Count == 1 && error.ErrorsMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            //Arange
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase();

            //Act
            Func<Task> act = async () => await useCase.Execute(request);

            //Aseert
            (await act.Should().ThrowAsync<ErrorOnValidationException>())
                .Where(error => error.ErrorsMessages.Count == 1 && error.ErrorsMessages.Contains(ResourceMessagesException.NAME_EMPTY));
        }
    }
}
