namespace dkt_learn_core.Services;

public interface IEmailService
{
    Task SendResetCodeAsync(string toEmail, string code);
}