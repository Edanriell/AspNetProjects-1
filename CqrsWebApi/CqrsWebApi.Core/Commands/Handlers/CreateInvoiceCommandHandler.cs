using CqrsWebApi.Core.Models.Dto;
using CqrsWebApi.Core.Services.Interfaces;
using MediatR;

namespace CqrsWebApi.Core.Commands.Handlers;

public class CreateInvoiceCommandHandler(IInvoiceService invoiceService)
	: IRequestHandler<CreateInvoiceCommand, InvoiceDto>
{
	public Task<InvoiceDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
	{
		return invoiceService.AddAsync(request.Invoice, cancellationToken);
	}
}