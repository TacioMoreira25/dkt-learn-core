using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dkt_learn_core.shared.Models.Dtos;
using dkt_learn_core.shared.Models.Models;
using DKT_Learn.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dkt_learn_core.Services;

public class AuthService : IAuthService
{
    public readonly IConfiguration configuration;
    private readonly AppDbContext context;

    public AuthService(IConfiguration configuration, AppDbContext context)
    {
        this.configuration = configuration;
        this.context = context;
    }
    
    public async Task<User?> RegisterAsync(UserDto request)
    {
        if(await context.Users.AnyAsync(u => u.Username == request.Username))
            return null;
        var user = new User();
        user.Username = request.Username;
        user.Email = request.Email;
        user.FotoUrl = null;
        
        var inviteCode = configuration.GetValue<string>("AppSettings:ProfessorInviteCode");
        string role = "Aluno"; 
        if (!string.IsNullOrEmpty(request.InviteCode))
        {
            if (request.InviteCode == inviteCode)
                role = "Professor";
            else
                return null; 
        }
        user.Roles = role;
        user.PasswordHash = new PasswordHasher<User>()
            .HashPassword(user, request.Password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
        
    public async Task<TokenResponseDto?> LoginAsync(UserDto request)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username 
                                                                  || u.Email == request.Username);
        if (user == null)
            return null;
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) 
            == PasswordVerificationResult.Failed)
            return null;
        var token = new TokenResponseDto
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshToken(user)
        };
        return token;
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenResponseDto request)
    {
        var user = await context.Users.FindAsync(request.UserId);
        if(user is null || user.RefreshToken != request.RefreshToken
           || user.RefreshTokenExpiry < DateTime.UtcNow)
           return null;
        var token = new TokenResponseDto
        {
            AccessToken = CreateToken(user), 
            RefreshToken = await GenerateAndSaveRefreshToken(user)
        };
        return token;
    }

    private async Task<string> GenerateAndSaveRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
        await context.SaveChangesAsync();
        return refreshToken;
    }
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Roles),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken
        (
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    
    public async Task<bool> LogoutAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
            return false;
        user.RefreshToken = string.Empty;
        await context.SaveChangesAsync();
        return true;
    }
    
    public async Task<User?> UpdateAsync(UserDto request, Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null)
            return null;
     
        user.Username = request.Username;
        user.Email = request.Email;
        user.FotoUrl = request.FotoUrl;
        
        var inviteCode = configuration.GetValue<string>("AppSettings:ProfessorInviteCode");
        string role = "Aluno"; 
        if (!string.IsNullOrEmpty(request.InviteCode))
        {
            if (request.InviteCode == inviteCode)
                role = "Professor";
            else
                return null; 
        }
        user.Roles = role;
        
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> DeleteUserAsync(UserDto request)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username 
                                                                  || u.Email == request.Username);
        if (user == null)
            return null;
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return user;
    }
}