using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.UseCases.User.Register
{
    public class RegisterUserUseCase
    {
        public ResponseRegisteredUserJson Execute(RequestRegisterUserJson requestUser)
        {
            //validação do request
            Validate(requestUser);

            //mapear request em uma entidade

            //criptografar senha

            //salvar no banco de dados

            return new ResponseRegisteredUserJson
            {
                Name = requestUser.Name,
            };
        }

        private void Validate(RequestRegisterUserJson requestUser)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(requestUser);

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
