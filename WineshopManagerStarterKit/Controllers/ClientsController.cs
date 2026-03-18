using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WineshopManagerStarterKit.Data;
using WineshopManagerStarterKit.Models;

namespace WineshopManagerStarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetAll()
    {
        return await _context.Clients
            .Include(c => c.Tickets) // Include related Tickets
            .ToListAsync();
    }

    // GET: api/clients/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _context.Clients
            .Include(c => c.Tickets) // Include related Tickets
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            return NotFound();
        }
        return client;
    }

    // POST: api/clients
    [HttpPost]
    public async Task<ActionResult<Client>> Create(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
    }

    // PUT: api/clients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }

        _context.Entry(client).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Clients.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/clients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var wine = await _context.Clients.FindAsync(id);

        if (wine == null)
        {
            return NotFound();
        }

        _context.Clients.Remove(wine);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
