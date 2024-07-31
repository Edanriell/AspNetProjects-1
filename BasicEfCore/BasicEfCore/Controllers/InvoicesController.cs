using BasicEfCore.Data;
using BasicEfCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicEfCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController(InvoiceDbContext context) : ControllerBase
{
    // The following code snippet shows how to retrieve all invoices from the database.
    // GET: api/Invoices
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
    // {
    //     if (context.Invoices == null)
    //     {
    //         return NotFound();
    //     }
    //     return await context.Invoices.ToListAsync();
    // }

    //The following code snippet shows how to retrieve all invoices from the database with a specific status.
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(InvoiceStatus? status)
    //{
    //     if (context.Invoices == null)
    //     {
    //         return NotFound();
    //     }
    //    return await context.Invoices.Where(x => status == null || x.Status == status).ToListAsync();
    //}

    // The following code snippet shows how to retrieve pages of invoices from the database.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int page = 1, int pageSize = 10,
        InvoiceStatus? status = null)
    {
        if (context.Invoices == null) return NotFound();
        // The AsQueryable() method is not required, as the DbSet<TEntity> class implements the IQueryable<TEntity> interface.
        return await context.Invoices.AsQueryable().Where(x => status == null || x.Status == status)
            .OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // GET: api/Invoices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        if (context.Invoices == null) return NotFound();
        var invoice = await context.Invoices.FindAsync(id);

        if (invoice == null) return NotFound();

        return invoice;
    }

    // POST: api/Invoices
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
    {
        if (context.Invoices == null) return Problem("Entity set 'InvoiceDbContext.Invoices'  is null.");
        context.Invoices.Add(invoice);
        // The preceding code is equivalent to the following code:
        //context.Entry(invoice).State = EntityState.Added;
        await context.SaveChangesAsync();

        return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
    }

    // PUT: api/Invoices/5
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();

        //context.Entry(invoice).State = EntityState.Modified;

        try
        {
            var invoiceToUpdate = await context.Invoices.FindAsync(id);
            if (invoiceToUpdate == null) return NotFound();

            // Old-school way to do things.
            // invoiceToUpdate.InvoiceNumber = invoice.InvoiceNumber;
            // invoiceToUpdate.ContactName = invoice.ContactName;
            // invoiceToUpdate.Description = invoice.Description;
            // invoiceToUpdate.Amount = invoice.Amount;
            // invoiceToUpdate.InvoiceDate = invoice.InvoiceDate;
            // invoiceToUpdate.DueDate = invoice.DueDate;
            // invoiceToUpdate.Status = invoice.Status;

            // Update only the properties that have changed
            // New more appropriate way.
            context.Entry(invoiceToUpdate).CurrentValues.SetValues(invoice);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Invoices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        if (context.Invoices == null) return NotFound();
        var invoice = await context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();

        // context.Entry(invoice).State = EntityState.Deleted;
        context.Invoices.Remove(invoice);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool InvoiceExists(Guid id)
    {
        return (context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}