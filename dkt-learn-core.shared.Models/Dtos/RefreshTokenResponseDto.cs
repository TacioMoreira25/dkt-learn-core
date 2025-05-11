namespace dkt_learn_core.shared.Models.Dtos;

public class RefreshTokenResponseDto
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}