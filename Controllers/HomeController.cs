using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Med.Models;
using System.Data.SqlClient;
using Med.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Azure;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace Med.Controllers
{
    [Authorize]
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

    //[HttpGet("/api/GetTests")]
    //public IActionResult GetTests()
    //{
    //    List<Test> tests = _db.Tests.ToList();
    //    return Ok(tests);
    //}

    //[HttpGet("/api/GetQuestions")]
    //public IActionResult GetQuestions()
    //{
    //    List<Question> qs = _db.Questions.ToList();
    //    return Ok(qs);
    //}


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

        [HttpGet("/api/GetRandomDrug")]
        public async Task<IActionResult> GetRandomDrug()
        {
            var currentUser = _db.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name));
            if (currentUser == null) return BadRequest("Wrong authorization");

            Random rnd = new Random();
            List<Test> tests = await _db.Tests
                .Include(t => t.PassedTests)
                .Where(t => t.PassedTests.Count == 0 || t.PassedTests.Any(pt => pt.UserId != currentUser.Id || pt.IsPassed != 1))
                .ToListAsync();
            if (tests.Count == 0) return NotFound("No tests yet.");

            Test rnd_test = tests.OrderBy(t => rnd.Next()).First();
            Drug drug = await _db.Drugs.FindAsync(rnd_test.DrugId);

            JObject response = new JObject();
            response.Add("test_id", rnd_test.Id);
            response.Add("points", rnd_test.Points);
            response.Add("drug_title", drug.Title);
            response.Add("drug_summary", drug.Summary);
            response.Add("drug_description", drug.Description);
            response.Add("video_url", drug.VideoUrl);
            response.Add("image_url", drug.ImageUrl);
            return Ok(response);
        }

        [HttpPost("/api/GetTest")]
        public async Task<IActionResult> GetTest([FromBody] JObject data)
        {
            var currentUser = _db.Users.FirstOrDefault(x => x.Email.Equals(User.Identity.Name));
            if (currentUser == null) return BadRequest("Wrong authorization");

            int test_id = Int32.Parse(data["test_id"].ToString());
            Test? test = await _db.Tests.FindAsync(test_id);
            if (test == null) return NotFound($"Test with id {test_id} doesn't exist.");

            List<Question> questions = await _db.Questions.Where(q => q.TestId.Equals(test_id)).ToListAsync();
            JArray jarrayObj = new JArray();

            foreach (var q in questions)
            {
                JObject item = new JObject();
                item.Add("question_text", q.QuestionText);
                item.Add("option1", q.Option1);
                item.Add("option2", q.Option2);
                item.Add("option3", q.Option3);
                item.Add("answer", q.Answer);
                jarrayObj.Add(item);
            }

            JObject response = new JObject();
            response.Add("test_id", test_id);
            response.Add("points", test.Points);
            response.Add("questions", jarrayObj);
            return Ok(response);
        }
    }

}
