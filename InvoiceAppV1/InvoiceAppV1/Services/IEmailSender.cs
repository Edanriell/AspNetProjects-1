namespace InvoiceAppV1.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body);
}