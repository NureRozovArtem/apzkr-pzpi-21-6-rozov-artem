using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehiclesTrackingSystem.Models;

namespace VehiclesTrackingSystem.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class DriversController : Controller
    {
        private readonly MyDbContext _context;

        public DriversController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("Page")]
        public IActionResult DriversPage()
        {
            var drivers = _context.Drivers.ToList();

            var userId = HttpContext.Session.GetInt32("UserId");
            ViewData["UserId"] = userId;

            return View("Drivers", drivers);
        }

        // GET: Drivers
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("DriversPage");
        }

        // GET: Drivers/AddDriver
        [HttpGet("AddDriver")]
        public IActionResult AddDriver()
        {
            return View("add-driver");
        }

        // POST: DriversApi
        [HttpPost]
        public async Task<IActionResult> PostDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DriversPage));
        }
    }
}