using dkt_learn_core.Services;
using dkt_learn_core.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

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
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(emailSettings.From));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Código de redefinição de senha";
        message.Body = new TextPart("plain") { Text = $"Seu código: {code}" };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(emailSettings.From, emailSettings.AppPassword);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}