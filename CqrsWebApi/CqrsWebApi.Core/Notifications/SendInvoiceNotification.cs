using MediatR;

namespace CqrsWebApi.Core.Notifications;

public class SendInvoiceNotification(Guid invoiceId) : INotification
{
	public Guid InvoiceId { get; set; } = invoiceId;
}