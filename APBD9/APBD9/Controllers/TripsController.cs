using APBD9.Data;
using APBD9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    public readonly ApbdContext _context;

    public TripsController(ApbdContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tripsQuery = _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var totalTrips = await _context.Trips.CountAsync();
        List<Trip> trips = await tripsQuery.ToListAsync();

        var result = new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = (int)Math.Ceiling(totalTrips / (double)pageSize),
            trips = trips.Select((Trip t) => new
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => c.Name),
                Clients = _context.ClientTrips
                    .Include(ct => ct.IdClientNavigation)
                    .Where(ct => ct.IdTrip == t.IdTrip)
                    .Select(ct => new
                        { FirstName = ct.IdClientNavigation.FirstName, LastName = ct.IdClientNavigation.LastName })
            })
        };

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        var clientTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == id);
        if (clientTrips)
        {
            return BadRequest("Cannot delete client with assigned trips.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] Data clientData)
    {
        var trip = await _context.Trips.FindAsync(idTrip);
        if (trip == null)
        {
            return NotFound($"Trip with ID {idTrip} not found.");
        }
        

        var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientData.Pesel);
        if (existingClient != null)
        {
            return BadRequest("Client with provided PESEL already exists.");
        }

        var isClientAssigned = await _context.ClientTrips.AnyAsync(ct =>
            ct.IdClientNavigation.Pesel == clientData.Pesel && ct.IdTrip == idTrip);
        if (isClientAssigned)
        {
            return BadRequest("Client is already assigned to this trip.");
        }

        var newClient = new Client
        {
            FirstName = clientData.FirstName,
            LastName = clientData.LastName,
            Email = clientData.Email,
            Telephone = clientData.Telephone,
            Pesel = clientData.Pesel
        };

        var clientTrip = new ClientTrip
        {
            IdClientNavigation = newClient,
            IdTripNavigation = trip,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientData.PaymentDate
        };

        _context.Clients.Add(newClient);

        _context.ClientTrips.Add(clientTrip);

        await _context.SaveChangesAsync();

        return Ok("Client assigned to trip successfully.");
    }
}

public class Data
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }
    public DateTime PaymentDate { get; set; }
}