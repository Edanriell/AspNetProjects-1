using CqrsWebApi.Core.Models.Dto;
using CqrsWebApi.Core.Services.Interfaces;
using MediatR;

namespace CqrsWebApi.Core.Queries.Handlers;

public class GetInvoiceByIdQueryHandler(IInvoiceService invoiceService)
	: IRequestHandler<GetInvoiceByIdQuery, InvoiceDto?>
{
	public Task<InvoiceDto?> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
	{
		return invoiceService.GetAsync(request.Id, cancellationToken);
	}
}