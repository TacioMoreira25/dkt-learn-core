using System.Security.Claims;
using dkt_learn_core.Services;
using dkt_learn_core.shared.Models.Dtos;
using DKT_Learn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dkt_learn_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly AppDbContext context;
        
        public UserController(IAuthService service, AppDbContext context)
        {
            this.service = service;
            this.context = context;
        }
        
        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(UserDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Usuário não identificado!");
            
            var userId = Guid.Parse(userIdClaim);
            var user = await service.UpdateAsync(request, userId);

            var response = new UserResponseDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Roles,
                FotoUrl = user.FotoUrl
            };
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-users")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        { 
            var user = await context.Users.ToListAsync();
            
            var response = user.Select(u => new UserResponseDto
            {
                Username = u.Username,
                Email = u.Email,
                Role = u.Roles,
                FotoUrl = u.FotoUrl
            }).ToList();
            
            return Ok(response);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-users")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> DeleteUsers(UserDto request)
        {
            var response = await service.DeleteUserAsync(request);
            if (response is null)
                return BadRequest("invalid username");
            
            return Ok("Usuario deletado com sucesso!");
        }
    }
}


