using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TypeWineController : ControllerBase
{
    private readonly AppDbContext _context;

    public TypeWineController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/typewines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TypeWine>>> GetAll()
    {
        return await _context.TypeWines.ToListAsync();
    }

    // GET: api/typewines/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TypeWine>> GetById(int id)
    {
        var typewine = await _context.TypeWines.FindAsync(id);

        if (typewine == null)
        {
            return NotFound();
        }

        return typewine;
    }

    // POST: api/typewines
    [HttpPost]
    public async Task<ActionResult<TypeWine>> Create(TypeWine typewine)
    {
        _context.TypeWines.Add(typewine);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = typewine.Id }, typewine);
    }

    // PUT: api/typewines/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TypeWine typewine)
    {
        if (id != typewine.Id)
        {
            return BadRequest();
        }

        _context.Entry(typewine).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.TypeWines.AnyAsync(w => w.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    // DELETE: api/typewines/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var typewine = await _context.TypeWines.FindAsync(id);

        if (typewine == null)
        {
            return NotFound();
        }

        _context.TypeWines.Remove(typewine);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
