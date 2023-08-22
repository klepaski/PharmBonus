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

        //{
        //  "title": "Naphazalin",
        //  "summary": "Nose spray",
        //  "description": "Spray for running nose",
        //  "videoUrl": "https://youtu.be/M8QKjDzb-Os",
        //  "imageUrl": "https://www.dropbox.com/scl/fi/i0vh1stcwlhcrw5nspsqo/form_196.jpg?rlkey=oq9tjifcx1kpfl4iqhkgm6fsp&dl=0"
        //}

    //[HttpGet("/api/SampleData")]
    //public async Task<IActionResult> Sample()
    //{
    //    Test test = new Test()
    //    {
    //        DrugId = 1,
    //        Points = 10
    //    };
    //    await _db.AddAsync(test);
    //    await _db.SaveChangesAsync();
    //    Question q1 = new Question()
    //    {
    //        TestId = 1,
    //        QuestionText = "What is the name of drug?",
    //        Option1 = "Remantadin",
    //        Option2 = "Loopy",
    //        Option3 = "Naphazalin",
    //        Answer = "Naphazalin"
    //    };
    //    Question q2 = new Question()
    //    {
    //        TestId = 1,
    //        QuestionText = "Is it dangerous?",
    //        Option1 = "No, at all",
    //        Option2 = "Very!",
    //        Option3 = "Safe if use according to instructions",
    //        Answer = "Safe if use according to instructions"
    //    };
    //    Question q3 = new Question()
    //    {
    //        TestId = 1,
    //        QuestionText = "Do you like it?",
    //        Option1 = "Maybe... I don't know",
    //        Option2 = "Hell NO!",
    //        Option3 = "Ohh yes!",
    //        Answer = "Ohh yes!"
    //    };
    //    await _db.AddRangeAsync(q1, q2, q3);
    //    await _db.SaveChangesAsync();
    //    return Ok("Test created");
    //}

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
    }

}
