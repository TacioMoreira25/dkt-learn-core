using dkt_learn_core.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace dkt_learn_core.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        emailSettings = options.Value;
    }

    public async Task SendResetCodeAsync(string toEmail, string code)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Endereço de e-mail inválido.", nameof(toEmail));

        using var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(emailSettings.From, emailSettings.Password),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(emailSettings.From, "DKT Learn"),
            Subject = "Código de redefinição de senha",
            Body = $"<p>Seu código de redefinição é: <strong>{code}</strong></p>",
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        await smtp.SendMailAsync(message);
    }
}
