namespace dkt_learn_core.shared.Models.Dtos;

public class ResetPasswordDto
{
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}