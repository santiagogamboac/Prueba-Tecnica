using Backend.Application.Contracts.Identity;
using Backend.Application.Models.Common;
using Backend.Application.Models.Identity;
using Backend.Application.Models.ViewModels.Access;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        public AuthController(IAuthService authService, IMediator mediator)
        {
            _authService = authService;
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.Login(request);

            if (response == null)
                return Unauthorized(new { message = "Usuario o contraseña inválidos" });

            return Ok(response);
        }
           

        [HttpPost("register")]
        [ProducesResponseType(typeof(ActionResult<RegistrationRequest>), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GeneralResponse>> Register([FromBody] RegistrationRequest request)
        {
            GeneralResponse response;

            try
            {
                return Ok( await _authService.Register(request));
                    
            }
            catch (Exception ex)
            {
                response = new GeneralResponse(false, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("refreshToken")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] TokenRequest request)
            => Ok(await _authService.RefreshToken(request));
    }
}
