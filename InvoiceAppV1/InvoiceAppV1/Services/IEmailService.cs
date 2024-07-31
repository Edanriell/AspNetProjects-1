using InvoiceAppV1.Models;

namespace InvoiceAppV1.Services;

public interface IEmailService
{
    (string to, string subject, string body) GenerateInvoiceEmail(Invoice invoice);
    Task SendEmailAsync(string to, string subject, string body);
}