namespace InvoiceAppV2.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body);
}