using CqrsWebApi.Core.Commands;
using CqrsWebApi.Core.Models.Dto;
using CqrsWebApi.Core.Notifications;
using CqrsWebApi.Core.Queries;
using CqrsWebApi.Core.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CqrsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController(IInvoiceService invoiceService, ISender mediatorSender, IPublisher mediatorPublisher)
	: ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<InvoiceWithoutItemsDto>>> GetInvoices()
	{
		var invoices = await invoiceService.GetAllListAsync();
		return Ok(invoices);
	}

	[HttpGet]
	[Route("paged")]
	public async Task<ActionResult<IEnumerable<InvoiceWithoutItemsDto>>> GetInvoices(int pageIndex, int pageSize)
	{
		var request = new GetInvoiceListQuery(pageIndex, pageSize);
		var invoices = await mediatorSender.Send(request);
		return Ok(invoices);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<InvoiceDto>> GetInvoice(Guid id)
	{
		//var invoice = await invoiceService.GetAsync(id);
		var invoice = await mediatorSender.Send(new GetInvoiceByIdQuery(id));
		return invoice == null ? NotFound() : Ok(invoice);
	}

	[HttpPost]
	public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateOrUpdateInvoiceDto invoiceDto)
	{
		//var invoice = await invoiceService.AddAsync(invoiceDto);
		var invoice = await mediatorSender.Send(new CreateInvoiceCommand(invoiceDto));
		await mediatorPublisher.Publish(new SendInvoiceNotification(invoice.Id));
		return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateInvoice(Guid id, CreateOrUpdateInvoiceDto invoiceDto)
	{
		try
		{
			await invoiceService.UpdateAsync(id, invoiceDto);
		}
		catch (InvalidOperationException)
		{
			return NotFound();
		}

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteInvoice(Guid id)
	{
		try
		{
			await invoiceService.DeleteAsync(id);
		}
		catch (InvalidOperationException)
		{
			return NotFound();
		}

		return NoContent();
	}
}