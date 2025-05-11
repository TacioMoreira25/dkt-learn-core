namespace dkt_learn_core.shared.Models.Dtos;

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password{ get; set; } = string.Empty;
    public string? InviteCode { get; set; }
}