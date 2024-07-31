using CqrsWebApi.Core.Models.Dto;
using CqrsWebApi.Core.Services.Interfaces;
using MediatR;

namespace CqrsWebApi.Core.Queries.Handlers;

public class GetInvoiceListQueryHandler(IInvoiceService invoiceService)
	: IRequestHandler<GetInvoiceListQuery, List<InvoiceWithoutItemsDto>>
{
	public Task<List<InvoiceWithoutItemsDto>> Handle(GetInvoiceListQuery request, CancellationToken cancellationToken)
	{
		return invoiceService.GetPagedListAsync(request.PageIndex, request.PageSize, cancellationToken);
	}
}