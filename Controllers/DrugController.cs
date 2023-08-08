using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Med.Models;
using System.Data.SqlClient;
using Med.DAL;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Med.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    public class DrugController : ControllerBase
    {
        private readonly ILogger<DrugController> _logger;
        private readonly ApplicationDbContext _db;

        public DrugController(ILogger<DrugController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]//[HttpGet(Name = "GetDrugs")]
        public IActionResult Get()
        {
            var drugs = _db.Drugs.ToList();
            if (drugs.Count == 0)
            {
                return NotFound("Drugs not available.");
            }
            return Ok(drugs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var drug = await _db.Drugs.FindAsync(id);
            if (drug == null)
            {
                return NotFound($"Drug with id {id} not found.");
            }
            return Ok(drug);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Drug drug)
        {
            await _db.AddAsync(drug);
            await _db.SaveChangesAsync();
            return Ok("Drug created.");
        }

        [HttpPut]
        public async Task<IActionResult> Put(Drug newDrug)
        {
            var drug = await _db.Drugs.FindAsync(newDrug.Id);
            if (newDrug == null)
            {
                return BadRequest("Model is invalid.");
            }
            if (drug == null)
            {
                return NotFound("Drug not found.");
            }
            drug.Title = newDrug.Title;
            drug.Summary = newDrug.Summary;
            drug.Description = newDrug.Description;
            drug.VideoUrl = newDrug.VideoUrl;
            drug.ImageUrl = newDrug.ImageUrl;
            await _db.SaveChangesAsync();
            return Ok("Drug updated.");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var drug = await _db.Drugs.FindAsync(id);
            if (drug == null)
            {
                return NotFound($"Drug with id {id} not found.");
            }
            _db.Drugs.Remove(drug);
            await _db.SaveChangesAsync();
            return Ok("Drug deleted.");
        }
    }
}
