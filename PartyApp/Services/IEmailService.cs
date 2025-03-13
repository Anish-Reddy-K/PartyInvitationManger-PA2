public interface IEmailService
{
    Task SendInvitationEmailAsync(string toEmail, string guestName, string partyDescription, DateTime partyDate, string partyLocation, int invitationId);
}