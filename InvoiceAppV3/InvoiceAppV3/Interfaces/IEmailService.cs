using InvoiceAppV3.Models;

namespace InvoiceAppV3.Interfaces;

public interface IEmailService
{
    (string to, string subject, string body) GenerateInvoiceEmail(Invoice invoice);
    Task SendEmailAsync(string to, string subject, string body);
}