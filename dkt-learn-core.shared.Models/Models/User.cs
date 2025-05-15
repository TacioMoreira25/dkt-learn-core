
namespace dkt_learn_core.shared.Models.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FotoUrl { get; set; } 
    public string PasswordHash { get; set; } = string.Empty;
    public string Roles { get; set; } = "Aluno";
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; }
    public string? PasswordResetCode { get; set; }
    public DateTime? ResetCodeExpiry { get; set; }

}