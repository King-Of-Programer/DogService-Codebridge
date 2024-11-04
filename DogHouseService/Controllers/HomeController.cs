using DogHouseService.Data;
using DogHouseService.Data.Entity;
using DogHouseService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogHouseService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly DataContext _context;
        public HomeController(DataContext context, IDogRepository dogRepository)
        {
            _context = context;
            _dogRepository = dogRepository;
        }

        [HttpGet("/ping")]
        public async Task<IActionResult> GetMessage()
        {
            return Ok("Dogshouseservice.Version1.0.1");
        }
        [HttpGet("/dogs")]
        public async Task<ActionResult<IEnumerable<Dog>>> GetDogs(
                string? attribute = null,
                string order = "asc",
                int? pageNumber = null,
                int? pageSize = null)
        {
            var dogsQuery = _context.Dogs.AsQueryable();

           
            if (!string.IsNullOrEmpty(attribute))
            {
                dogsQuery = order.ToLower() == "desc"
                    ? dogsQuery.OrderByDescending(d => EF.Property<object>(d, attribute))
                    : dogsQuery.OrderBy(d => EF.Property<object>(d, attribute));
            }

           
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                dogsQuery = dogsQuery
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

                int totalItems = await _context.Dogs.CountAsync();
                Response.Headers.Add("X-Total-Count", totalItems.ToString());
                Response.Headers.Add("X-Page-Number", pageNumber.Value.ToString());
                Response.Headers.Add("X-Page-Size", pageSize.Value.ToString());
            }

            return Ok(await dogsQuery.ToListAsync());
        }
        
        [HttpPost("/dog")]
        public async Task<ActionResult<Dog>> CreateDog([FromBody] Dog dog)
        {
            if (dog == null)
            {
                return BadRequest("Invalid JSON format.");
            }

            
            if (string.IsNullOrEmpty(dog.name))
            {
                return BadRequest("Dog name is required.");
            }

            if (await _dogRepository.ExistsByNameAsync(dog.name))
            {
                return Conflict("A dog with the same name already exists.");
            }

            if (dog.tail_length < 0)
            {
                return BadRequest("Tail length cannot be negative.");
            }

           
            await _dogRepository.AddDogAsync(dog);

            return CreatedAtAction(nameof(CreateDog), dog);
        }


    }
}
