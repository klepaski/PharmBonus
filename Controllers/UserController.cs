using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Med.Models;
using Med.Services;
using System.Data.SqlClient;
using Med.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Newtonsoft.Json.Linq;

namespace Med.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;

        public UserController(ILogger<UserController> logger, ApplicationDbContext db, IEmailService emailService)
        {
            _logger = logger;
            _db = db;
            _emailService = emailService;
        }

        //[HttpPost("/api/test")]
        //public IActionResult Test(Object obj)
        //{
        //    return Ok(obj);
        //}

        [HttpGet("/api/Email")]
        public IActionResult Email(int userId)
        {
            User? user = _db.Users.Find(userId);
            if (user == null)
            {
                return NotFound($"User with id {userId} doesn't exist.");
            }
            user.IsEmailConfirmed = 1;
            _db.SaveChanges();
            return Ok("Email confirmed.");
        }

        [HttpPost("/token")]
        public IActionResult Token([FromBody] JObject data)
        {
            string email = data["email"].ToString();
            string password = data["password"].ToString();

            var identity = GetIdentity(email, password);
            if (identity == null)
            {
                return BadRequest("Invalid email or password.");
            }
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            JObject response = new JObject();
            response.Add("access_token", encodedJwt);
            response.Add("email", identity.Name);
            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = _db.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (user == null) return null;
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _db.Users.ToList();
            if (users.Count == 0)
            {
                return NotFound("Users not found.");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            return Ok(user);
        }

        //ПОДТВЕРЖДЕНИЕ ПОЧТЫ!!

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            if (_db.Users.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                return BadRequest("User with this email already exists.");
            }
            User newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Region = user.Region,
                City = user.City,
                Category = user.Category,
                Password = user.Password,
                Role = user.Role,

                IsVerified = 0,
                IsBlocked = 0,
                RegistrationDate = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                Count = 0
            };
            await _db.AddAsync(newUser);
            await _db.SaveChangesAsync();
            _emailService.SendConfirmationEmail(newUser.Id, newUser.FirstName, newUser.LastName, newUser.Email);
            return Ok("User created.");
        }


        [HttpPut]
        public async Task<IActionResult> Put(User newUser)
        {
            var user = await _db.Users.FindAsync(newUser.Id);
            if (newUser == null)
            {
                return BadRequest("Model is invalid.");
            }
            if (user == null)
            {
                return NotFound("User not found.");
            }
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Region = newUser.Region;
            user.City = newUser.City;
            user.Category = newUser.Category;
            user.Password = newUser.Password;
            user.Count = newUser.Count;
            await _db.SaveChangesAsync();
            return Ok("User updated.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Ok("User deleted.");
        }
    }
}
