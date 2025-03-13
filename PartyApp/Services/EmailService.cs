using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendInvitationEmailAsync(string toEmail, string guestName, string partyDescription, DateTime partyDate, string partyLocation, int invitationId)
    {
        // Similar to the email example code provided
        string fromAddress = _configuration["Email:Address"];
        string password = _configuration["Email:Password"];

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromAddress, password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            EnableSsl = true,
        };

        string responseUrl = _configuration["AppUrl"] + $"/Invitation/Respond/{invitationId}";

        var mailMessage = new MailMessage()
        {
            From = new MailAddress(fromAddress),
            Subject = $"You're invited to {partyDescription}!",
            Body = $@"
                <h1>Hello {guestName},</h1>
                <p>You are invited to {partyDescription}!</p>
                <p><strong>Date:</strong> {partyDate.ToShortDateString()}</p>
                <p><strong>Location:</strong> {partyLocation}</p>
                <p>Please <a href='{responseUrl}'>click here</a> to respond.</p>",
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        return Task.Run(() => smtpClient.Send(mailMessage));
    }
}