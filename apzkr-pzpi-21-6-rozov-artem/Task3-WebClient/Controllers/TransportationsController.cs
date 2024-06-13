using Microsoft.AspNetCore.Mvc;
using VehiclesTrackingSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VehiclesTrackingSystem.Controllers
{
    [Route("[controller]")]
    public class TransportationsController : Controller
    {
        private readonly MyDbContext _context;

        public TransportationsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Transportations/Page/user/5
        [HttpGet("Page/user/{userId}")]
        public async Task<IActionResult> GetTransportationsByUserIdPage(int userId)
        {
            var transportations = await _context.Transportations
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (transportations == null || !transportations.Any())
            {
                return View("NoTransportationsForUser");
            }

            return View("Transportations", transportations);
        }
    }
}
