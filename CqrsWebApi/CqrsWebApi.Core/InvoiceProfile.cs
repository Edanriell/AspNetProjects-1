using AutoMapper;
using CqrsWebApi.Core.Models;
using CqrsWebApi.Core.Models.Dto;

namespace CqrsWebApi.Core;

public class InvoiceProfile : Profile
{
	public InvoiceProfile()
	{
		CreateMap<CreateOrUpdateInvoiceItemDto, InvoiceItem>();
		CreateMap<InvoiceItem, InvoiceItemDto>();
		CreateMap<CreateOrUpdateInvoiceDto, Invoice>();
		CreateMap<Invoice, InvoiceWithoutItemsDto>();
		CreateMap<Invoice, InvoiceDto>();
	}
}