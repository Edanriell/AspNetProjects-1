using CqrsWebApi.Core.Models.Dto;
using MediatR;

namespace CqrsWebApi.Core.Commands;

public class CreateInvoiceCommand(CreateOrUpdateInvoiceDto invoice) : IRequest<InvoiceDto>
{
	public CreateOrUpdateInvoiceDto Invoice { get; set; } = invoice;
}