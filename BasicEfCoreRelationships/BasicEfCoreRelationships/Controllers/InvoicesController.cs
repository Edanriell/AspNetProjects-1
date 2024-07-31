using BasicEfCoreRelationships.Data;
using BasicEfCoreRelationships.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicEfCoreRelationships.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController(SampleDbContext context) : ControllerBase
{
    // GET: api/Invoices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int page = 1, int pageSize = 10,
        InvoiceStatus? status = null)
    {
        if (context.Invoices == null) return NotFound();

        // The Include() method is a convenient way to include dependent entities. However, it may
        // cause performance issues when the collection of dependent entities is large. For example, a post
        // may have hundreds or thousands of comments. It is not a good idea to include all comments
        // in the query result for a list page. In this case, it is not necessary to include dependent entities
        // in the query

        return await context.Invoices
            .Include(x => x.InvoiceItems)
            .Where(x => status == null || x.Status == status)
            .OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            // query includes Invoice data in each row of the result. For some scenarios, it
            // may cause a so-called Cartesian explosion problem, which means the amount of duplicated
            // data in the result is too large and may cause performance issues. In this case, we can split the
            // queries into two steps. First, we query the invoices, and then we query the invoice items.
            //.AsSplitQuery()
            // If we  want to execute a specific query in a single query, you can use the AsSingleQuery() method
            // .AsSingleQuery()
            .ToListAsync();
    }

    // GET: api/Invoices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        if (context.Invoices == null) return NotFound();

        //var invoice = await _context.Invoices.FindAsync(id);
        var invoice = await context.Invoices.Include(x => x.InvoiceItems).SingleOrDefaultAsync(x => x.Id == id);

        if (invoice == null) return NotFound();

        return invoice;
    }

    // PUT: api/Invoices/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();

        context.Entry(invoice).State = EntityState.Modified;

        try
        {
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

    // POST: api/Invoices
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
    {
        if (context.Invoices == null) return Problem("Entity set 'InvoiceDbContext.Invoices'  is null.");

        context.Invoices.Add(invoice);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
    }

    // DELETE: api/Invoices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        if (context.Invoices == null) return NotFound();

        //await _context.Invoices.Where(x => x.Id == id).ExecuteDeleteAsync();
        var invoice = await context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();
        context.Invoices.Remove(invoice);
        return NoContent();
    }

    private bool InvoiceExists(Guid id)
    {
        return (context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}