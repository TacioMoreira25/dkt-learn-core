using System.Security.Claims;
using dkt_learn_core.Services;
using dkt_learn_core.shared.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserDto request)
        {
            var user = await service.RegisterAsync(request);
            if (user == null)
                return BadRequest("user already exists!");

            var response = new UserResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Roles,
                FotoUrl = user.FotoUrl
            };

            return Ok(response);
        }

        
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
           var token = await service.LoginAsync(request);
           if (token is null)
               return BadRequest("invalid username/Email or password!");
            return Ok(token);
        }
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenResponseDto request)
        {
            var token = await service.RefreshTokenAsync(request);
            if (token is null)
                return BadRequest("invalid/expired token");
            return Ok(token);
        }
        
        
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Usuário não identificado!");

            var userId = Guid.Parse(userIdClaim);
            var result = await service.LogoutAsync(userId);

            if (!result)
                return NotFound("Usuário não encontrado!");

            return Ok(new { message = "Logout realizado com sucesso!" });
        }

        
        [HttpGet("Auth-endpoint")]
        [Authorize]
        public ActionResult<string> AuthCheck()
        {
            return Ok();
        }
        
        [HttpGet("Admin-endpoint")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCheck()
        {
            return Ok();
        }
       
    }
}
