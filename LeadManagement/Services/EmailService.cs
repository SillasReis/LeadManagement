using LeadManagement.Interfaces;

namespace LeadManagement.Services;

public class EmailService : IEmailService
{
    public void SendEmail(string dest, string subject, string body)
    {
        Console.WriteLine($"Sending email to {dest}\nSubject: {subject}\nBody: {body}");
    }
}
