using AdvancedEfCore.Data;
using AdvancedEfCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AdvancedEfCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoicesController(InvoiceDbContext context, ILogger<InvoicesController> logger)
    : ControllerBase
{
    // GET: api/Invoices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int page = 1, int pageSize = 10,
        InvoiceStatus? status = null)
    {
        // The first difference between IQueryable and IEnumerable is that IQueryable is in the
        // System.Linq namespace, while IEnumerable is in the System.Collections namespace.
        // The IQueryable interface inherits from the IEnumerable interface, so IQueryable can do
        // everything that IEnumerable does. But why do we need the IQueryable interface?

        // One of the key differences between IQueryable and IEnumerable is that IQueryable is
        // used to query data from a specific data source, such as a database. IEnumerable is used to iterate
        // through a collection in memory. When we use IQueryable, the query will be translated into a
        // specific query language, such as SQL, and executed against the data source to get the results when
        // we call the ToList() (or ToAway()) method or iterate the items in the collection.

        if (context.Invoices == null) return NotFound();
        // Use IQueryable
        logger.LogInformation("Creating the IQueryable...");
        var list1 = context.Invoices.Where(x => status == null || x.Status == status);
        logger.LogInformation("IQueryable created");
        logger.LogInformation("Query the result using IQueryable...");
        var query1 = list1.OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        logger.LogInformation("Execute the query using IQueryable");
        var result1 = await query1.ToListAsync();
        logger.LogInformation("Result created using IQueryable");

        // Use IEnumerable
        logger.LogInformation("Creating the IEnumerable...");
        var list2 = context.Invoices.Where(x => status == null || x.Status == status).AsEnumerable();
        logger.LogInformation("IEnumerable created");
        logger.LogInformation("Query the result using IEnumerable...");
        var query2 = list2.OrderByDescending(x => x.InvoiceDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        logger.LogInformation("Execute the query using IEnumerable");
        var result2 = query2.ToList();
        logger.LogInformation("Result created using IEnumerable");

        return result1;

        // Look at the logs. The generated SQL query does not contain the ORDER BY, OFFSET, and FETCH
        // NEXT clauses, which means the query fetched all the invoices from the database and then filtered,
        // sorted, and paged the invoices in memory. If we have a large number of entities in the database, the
        // second query will be very slow and inefficient.
        // Now, we can see the difference between the two interfaces. The IQueryable interface is a deferred
        // execution query, which means the query is not executed when we add more conditions to the query.
        // The query will be executed against the database when we call the ToList() or ToArray() methods
        // or iterate the items in the collection. So, in complex and heavy queries, we should always use the
        // IQueryable interface to avoid fetching all the data from the database. Be careful when you call
        // the ToList() or ToArray() methods because ToList() or ToArray() (and their async
        // overloads) will execute the query immediately.
        // In this section, we explored the differences between IQueryable and IEnumerable. It is important
        // to understand why we should use IQueryable instead of IEnumerable when querying the
        // database for complex and heavy queries. Loading all the data from the database can cause performance
        // issues if there are a large number of entities in the database.
    }

    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<IEnumerable<Invoice>>> SearchInvoices(string search)
    {
        if (context.Invoices == null) return NotFound();

        var list = await context.Invoices
            // The below code will throw an exception if the CalculatedTax method is not static
            //.Where(x => (x.ContactName.Contains(search) || x.InvoiceNumber.Contains(search)) && CalculateTax(x.Amount) > 10)
            .Where(x => x.ContactName.Contains(search) || x.InvoiceNumber.Contains(search))
            .Select(x => new Invoice
            {
                Id = x.Id,
                InvoiceNumber = x.InvoiceNumber,
                ContactName = x.ContactName,
                // The below conversion will be executed on the client side
                Description = $"Tax: ${CalculateTax(x.Amount)}. {x.Description}",
                Amount = x.Amount,
                InvoiceDate = x.InvoiceDate,
                DueDate = x.DueDate,
                Status = x.Status
            })
            .ToListAsync();
        return list;
    }

    private static decimal CalculateTax(decimal amount)
    {
        return amount * 0.15m;
    }

    [HttpGet]
    [Route("paid")]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetPaidInvoices()
    {
        if (context.Invoices == null) return NotFound();

        var list = await context.Invoices
            .FromSql($"SELECT * FROM Invoices WHERE Status = 2")
            .ToListAsync();
        return list;
    }

    [HttpGet]
    [Route("status")]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(string status)
    {
        if (context.Invoices == null) return NotFound();

        var list = await context.Invoices
            .FromSql($"SELECT * FROM Invoices WHERE Status = {status}")
            .ToListAsync();
        return list;
    }

    [HttpGet]
    [Route("free-search")]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(string propertyName, string propertyValue)
    {
        if (context.Invoices == null) return NotFound();

        var value = new SqlParameter("value", propertyValue);

        var list = await context.Invoices
            .FromSqlRaw($"SELECT * FROM Invoices WHERE {propertyName} = @value", value)
            .ToListAsync();
        return list;
    }

    [HttpGet]
    [Route("ids")]
    public ActionResult<IEnumerable<Guid>> GetInvoicesIds(string status)
    {
        if (context.Invoices == null) return NotFound();

        var result = context.Database
            .SqlQuery<Guid>($"SELECT Id FROM Invoices WHERE Status = {status}")
            .ToList();
        return result;
    }


    // GET: api/Invoices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
    {
        // ORM frameworks such as EF Core were created to solve these problems. They not only simplify the
        // process of executing SQL queries and mapping the results to model objects but also provide the ability
        // to track changes made to the entities returned by queries. When changes are saved, EF Core generates
        // the appropriate SQL queries to update the database. This is called tracking and is a significant benefit
        // of using an ORM framework such as EF Core.
        if (context.Invoices == null) return NotFound();
        logger.LogInformation($"Invoice {id} is loading from the database.");
        var invoice = await context.Invoices.FindAsync(id);
        logger.LogInformation($"Invoice {invoice?.Id} is loaded from the database.");

        logger.LogInformation($"Invoice {id} is loading from the context.");
        invoice = await context.Invoices.FindAsync(id);
        logger.LogInformation($"Invoice {invoice?.Id} is loaded from the context.");

        // When we call the context.Invoices.FindAsync(id) method for the first time, EF Core
        // will query the database and return the Invoice entity. The second time, EF Core will return the
        // Invoice entity from the context because the Invoice entity is already in the context.

        invoice = await context.Invoices.FirstOrDefaultAsync(x => x.Id == id);

        // Use no-tracking query to improve performance.
        // var invoice = await _context.Invoices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (invoice == null) return NotFound();

        return invoice;

        // Find() versus Single()
        // When we get an entity from the database by its primary key, we can use the Find() or
        // FindAsync() methods. Also, we can use the Single() or SingleOrDefault()
        // methods. They are similar, but they are not the same. The Find() and FindAsync()
        // methods are methods of the DbSet class. If an entity with the given primary key
        // values is being tracked by the context, the Find() or FindAsync() methods will
        // return the tracked entity without making a request to the database. Otherwise, EF
        // Core will make a query to the database to get the entity, attach it to the context, and
        // return it. But if you use the Single() or SingleOrDefault() methods, EF
        // Core will always make a query to the database to get the entity. The same is true for the
        // First() and FirstOrDefault() methods. So, the Find() and FindAsync()
        // methods are more efficient for getting an entity by its primary key. But in rare cases,
        // Find() and FindAsync() may return outdated data if the entity is updated in the
        // database after it is loaded into the context. For example, if you use the bulk-update
        // ExecuteUpdateAsync() method, the update will not be tracked by DbContext.
        // Then, if you use Find() or FindAsync() to get the entity from DbContext, you will
        // get the outdated data. In this case, you should use Single() or SingleOrDefault()
        // to get the entity from the database again. In most cases, you can use the Find() or
        // FindAsync() methods to get an entity by its primary key when you are sure the entity is
        // always tracked by the DbContext.

        // An entity has one of the following EntityState values: Detached, Added, Unchanged,
        // Modified, or Deleted.
    }

    // PUT: api/Invoices/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
    {
        if (id != invoice.Id) return BadRequest();

        // The below code will update all the fields
        //_context.Entry(invoice).State = EntityState.Modified;
        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!InvoiceExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}

        // The below code will update only the fields that have changed
        // var invoiceToUpdate = await _context.Invoices.FindAsync(id);
        // if (invoiceToUpdate == null)
        // {
        //     return NotFound();
        // }
        // invoiceToUpdate.InvoiceNumber = invoice.InvoiceNumber;
        // invoiceToUpdate.ContactName = invoice.ContactName;
        // invoiceToUpdate.Description = invoice.Description;
        // invoiceToUpdate.Amount = invoice.Amount;
        // invoiceToUpdate.InvoiceDate = invoice.InvoiceDate;
        // invoiceToUpdate.DueDate = invoice.DueDate;
        // invoiceToUpdate.Status = invoice.Status;

        // A better way to update only the fields that have changed
        var invoiceToUpdate = await context.Invoices.FindAsync(id);
        if (invoiceToUpdate == null) return NotFound();
        // Update only the properties that have changed
        context.Entry(invoiceToUpdate).CurrentValues.SetValues(invoice);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!InvoiceExists(id)) return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpPut]
    [Route("status/overdue")]
    public async Task<ActionResult> UpdateInvoicesStatusAsOverdue(DateTime date)
    {
        var result = await context.Invoices
            .Where(i => i.InvoiceDate < date && i.Status == InvoiceStatus.AwaitPayment)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, InvoiceStatus.Overdue));
        return Ok();
    }

    // POST: api/Invoices
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
    {
        if (context.Invoices == null) return Problem("Entity set 'InvoiceDbContext.Invoices'  is null.");

        context.Invoices.Add(invoice);
        // The above line is equivalent to the following two lines:
        //_context.Entry(invoice).State = EntityState.Added;
        await context.SaveChangesAsync();

        return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
    }

    // DELETE: api/Invoices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInvoice(Guid id)
    {
        if (context.Invoices == null) return NotFound();

        // var invoice = await _context.Invoices.FindAsync(id);
        // if (invoice == null)
        // {
        //     return NotFound();
        // }

        // _context.Invoices.Remove(invoice);
        // The above line is equivalent to the following two lines:
        //_context.Entry(invoice).State = EntityState.Deleted;

        // The following way does not need to find the entity first.
        // var invoice = new Invoice { Id = id };
        // _context.Invoices.Remove(invoice);

        //await _context.SaveChangesAsync();

        // Or you can use ExecuteSql method to execute a raw SQL query.
        //await _context.Database.ExecuteSqlAsync($"DELETE FROM Invoices WHERE Id = {id}");

        // Another way to delete an entity is to use the `ExecuteDeletAsync` method. It is available from EF Core 7.0.
        await context.Invoices.Where(x => x.Id == id).ExecuteDeleteAsync();

        return NoContent();
    }

    private bool InvoiceExists(Guid id)
    {
        return (context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}