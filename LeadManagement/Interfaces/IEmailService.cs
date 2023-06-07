namespace LeadManagement.Interfaces;

public interface IEmailService
{
    void SendEmail(string dest, string subject, string body);
}
