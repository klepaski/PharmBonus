using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Med.Models;
using System.Data.SqlClient;
using Med.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Azure;
using Newtonsoft.Json.Linq;

namespace Med.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var currentUser = _db.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name));
            if (currentUser == null) return BadRequest("Wrong authorization");

            JObject response = new JObject();
            response.Add("first_name", currentUser.FirstName);
            response.Add("last_name", currentUser.LastName);
            response.Add("user_counter", currentUser.Count);
            return Ok(response);
        }
    }
}
