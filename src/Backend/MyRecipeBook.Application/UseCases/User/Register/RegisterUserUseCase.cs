using AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;
        private readonly PasswordEncripter _passwordEncripter;

        public RegisterUserUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IUserWriteOnlyRepository userWriteOnlyRepository,   
            IUnityOfWork unityOfWork,   
            IMapper mapper, 
            PasswordEncripter passwordEncripter)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _unityOfWork = unityOfWork;
            _mapper = mapper;
            _passwordEncripter = passwordEncripter;
        }


        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson requestUser)
        {
            await Validate(requestUser);

            var user = _mapper.Map<Domain.Entities.User>(requestUser);

            user.Password = _passwordEncripter.Encrypt(requestUser.Password);

            await _userWriteOnlyRepository.Add(user);
            await _unityOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = requestUser.Name,
            };
        }

        private async Task Validate(RequestRegisterUserJson requestUser)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(requestUser);

            var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(requestUser.Email);
            if (emailExist)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
