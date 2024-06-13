using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VehiclesTrackingSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehiclesTrackingSystem.Resources;

namespace VehiclesTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportationsApiController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public TransportationsApiController(MyDbContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        // GET: api/Transportations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transportation>>> GetTransportations()
        {
            return await _context.Transportations.ToListAsync();
        }

        // GET: api/Transportations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transportation>> GetTransportation(int id)
        {
            var transportation = await _context.Transportations.FindAsync(id);

            if (transportation == null)
            {
                return NotFound(new
                {
                    message = _localizer["TransportationNotFound"]
                });
            }

            return transportation;
        }

        // POST: api/Transportations
        [HttpPost]
        public async Task<ActionResult<Transportation>> PostTransportation(Transportation transportation)
        {
            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransportation), new { id = transportation.TransportationId }, transportation);
        }

        // PUT: api/Transportations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportation(int id, Transportation transportation)
        {
            if (id != transportation.TransportationId)
            {
                return BadRequest();
            }

            _context.Entry(transportation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportationExists(id))
                {
                    return NotFound(new
                    {
                        message = _localizer["TransportationNotFound"]
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Transportations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportation(int id)
        {
            var transportation = await _context.Transportations.FindAsync(id);
            if (transportation == null)
            {
                return NotFound(new
                {
                    message = _localizer["TransportationNotFound"]
                });
            }

            _context.Transportations.Remove(transportation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Transportations/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Transportation>>> GetTransportationsByUserId(int userId)
        {
            var transportations = await _context.Transportations.Where(t => t.UserId == userId).ToListAsync();

            if (transportations == null || !transportations.Any())
            {
                return NotFound(new
                {
                    message = _localizer["NoTransportationsForUser"]
                });
            }

            return transportations;
        }

        private bool TransportationExists(int id)
        {
            return _context.Transportations.Any(e => e.TransportationId == id);
        }
    }
}
