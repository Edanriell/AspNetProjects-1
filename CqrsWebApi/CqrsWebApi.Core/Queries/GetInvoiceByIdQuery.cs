using CqrsWebApi.Core.Models.Dto;
using MediatR;

namespace CqrsWebApi.Core.Queries;

public class GetInvoiceByIdQuery(Guid id) : IRequest<InvoiceDto?>
{
	public Guid Id { get; set; } = id;
}