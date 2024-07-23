using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonManagment.API.Authority;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonManagment.API.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiresAt,_configuration.GetValue<string>("SecretKey")),
                    expires_at = expiresAt
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                return new UnauthorizedObjectResult(problemDetails);
            }

        }
        
    }
}
