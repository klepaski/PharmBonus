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
using System.Text.Json;
using System;

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

        [HttpPost("/api/test")]
        public IActionResult Test(Object obj)
        {
            return Ok(obj);
        }

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
        public IActionResult Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest("Invalid username or password.");
            }
            var now = DateTime.UtcNow;
            //создаем jwt-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(JsonSerializer.Serialize(response));
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _db.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
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
            if (_db.Users.FirstOrDefault(x => x.UserName == user.UserName) != null)
            {
                return BadRequest("User with this username already exists.");
            }
            if (_db.Users.FirstOrDefault(x => x.Email == user.Email) != null)
            {
                return BadRequest("User with this email already exists.");
            }
            User newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
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

        /*МЕНЯЕТ САМ СЕБЯ В ЛИЧНОМ КАБИНЕТЕ*/
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
            if (_db.Users.FirstOrDefault(x => x.UserName == newUser.UserName) != null)
            {
                return BadRequest("User with this username already exists.");
            }
            if (_db.Users.FirstOrDefault(x => x.Email == newUser.Email) != null)
            {
                return BadRequest("User with this email already exists.");
            }
            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            user.Region = newUser.Region;
            user.City = newUser.City;
            user.Category = newUser.Category;
            user.Password = newUser.Password;
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
