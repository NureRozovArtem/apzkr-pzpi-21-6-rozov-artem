using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VehiclesTrackingSystem.Models;
using VehiclesTrackingSystem.Resources;

namespace VehiclesTrackingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IStringLocalizer<LoginView> _localizer;

        public UsersController(MyDbContext context, IStringLocalizer<LoginView> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet("login")]
        public IActionResult LoginView()
        {
            Console.WriteLine("loginTitle = " + _localizer["loginTitle"]);
            ViewData["loginTitle"] = _localizer["loginTitle"];
            ViewData["usernamePlaceholder"] = _localizer["usernamePlaceholder"];
            ViewData["passwordPlaceholder"] = _localizer["passwordPlaceholder"];
            ViewData["loginButton"] = _localizer["loginButton"];
            ViewData["registerLinkText"] = _localizer["registerLinkText"];
            ViewData["registerLink"] = _localizer["registerLink"];
            return View("Index");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Користувач не знайден або неправильні дані." });
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);

            return Ok(new { userId = user.UserId });
        }

        [HttpGet("register")]
        public IActionResult RegisterView()
        {
            return View("Registration");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registerModel)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Login == registerModel.Login))
                {
                    return Conflict("User with this login already exists.");
                }

                var newUser = new User
                {
                    Login = registerModel.Login,
                    Password = registerModel.Password,
                    Email = registerModel.Email,
                    UserType = registerModel.UserType
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("Registration successful!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Користувача не знайдено.");
            }

            return Ok(user);
        }
    }
}
