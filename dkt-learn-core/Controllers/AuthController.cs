using System.Net.Mail;
using System.Security.Claims;
using dkt_learn_core.Services;
using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;
using DKT_Learn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly IEmailService emailService;
        public readonly AppDbContext context;
        public AuthController(IAuthService service, IEmailService emailService, AppDbContext context)
        {
            this.service = service;
            this.emailService = emailService;
            this.context = context;
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
        
        [HttpPost("request-reset-code")]
        public async Task<IActionResult> RequestResetCode(RequestResetCodeDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is null)
                return Ok("Usuário não encontrado."); 
    
            try
            {
                // Validação de e-mail
                var emailAddr = new MailAddress(user.Email);
            }
            catch
            {
                return BadRequest("E-mail inválido no cadastro.");
            }

            var code = new Random().Next(100000, 999999).ToString();

            user.PasswordResetCode = code;
            user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(5);

            await context.SaveChangesAsync();

            await emailService.SendResetCodeAsync(user.Email, code);

            return Ok("Código enviado.");
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.PasswordResetCode == dto.Code);

            if (user == null || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest("Código inválido ou expirado.");

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, dto.NewPassword);

            user.PasswordResetCode = null;
            user.ResetCodeExpiry = null;

            await context.SaveChangesAsync();

            return Ok("Senha redefinida com sucesso.");
        }


       
    }
}
