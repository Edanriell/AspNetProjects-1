using InvoiceAppV2.Models;

namespace InvoiceAppV2.Interfaces;

public interface IEmailService
{
    (string to, string subject, string body) GenerateInvoiceEmail(Invoice invoice);
    Task SendEmailAsync(string to, string subject, string body);
}