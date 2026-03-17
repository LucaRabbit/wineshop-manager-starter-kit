using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WinesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WinesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/wines
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Wine>>> GetAll()
    {
        return await _context.Wines.ToListAsync();
    }

    // GET: api/wines/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Wine>> GetById(int id)
    {
        var wine = await _context.Wines.FindAsync(id);

        if (wine == null)
        {
            return NotFound();
        }

        return wine;
    }

    // POST: api/wines
    [HttpPost]
    public async Task<ActionResult<Wine>> Create(Wine wine)
    {
        _context.Wines.Add(wine);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = wine.Id }, wine);
    }

    // PUT: api/wines/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Wine wine)
    {
        if (id != wine.Id)
        {
            return BadRequest();
        }

        _context.Entry(wine).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Wines.AnyAsync(w => w.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/wines/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var wine = await _context.Wines.FindAsync(id);

        if (wine == null)
        {
            return NotFound();
        }

        _context.Wines.Remove(wine);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
