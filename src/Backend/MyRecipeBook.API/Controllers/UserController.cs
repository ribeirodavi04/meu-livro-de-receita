using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody]RequestRegisterUserJson requestUser)
        {         
            var result = await useCase.Execute(requestUser);

            return Created(string.Empty, result);            
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUser]
        public async Task<ActionResult> GetUserProfile([FromServices] IGetUserProfileUseCase useCase)
        {
            var result = await useCase.Execute();

            return Ok(result);
        }
    }
}
