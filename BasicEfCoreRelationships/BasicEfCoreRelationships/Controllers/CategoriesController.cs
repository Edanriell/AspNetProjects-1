using BasicEfCoreRelationships.Data;
using BasicEfCoreRelationships.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicEfCoreRelationships.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(SampleDbContext context) : ControllerBase
{
    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        if (context.Categories == null) return NotFound();

        return await context.Categories.ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        if (context.Categories == null) return NotFound();

        var category = await context.Categories.FindAsync(id);

        if (category == null) return NotFound();

        return category;
    }

    // PUT: api/Categories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(Guid id, Category category)
    {
        if (id != category.Id) return BadRequest();

        context.Entry(category).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Categories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        if (context.Categories == null) return Problem("Entity set 'InvoiceDbContext.Categories'  is null.");

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        if (context.Categories == null) return NotFound();

        // When DeleteBehavior.ClientSetNull
        // In the CategoriesController class, you can find the DeleteCategory() method.
        // It is similar to the DeleteInvoice() method in the InvoicesController class. The
        // only difference is that we need to remove the relationship between the category and the blog
        // posts before deleting the category
        var category = await context.Categories.Include(x => x.Posts).SingleOrDefaultAsync(x => x.Id == id);
        if (category == null) return NotFound();
        category.Posts.Clear();
        // Or you can update the posts to set the category to null
        // foreach (var post in category.Posts)
        // {
        //     post.Category = null;
        // }
        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(Guid id)
    {
        return (context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}