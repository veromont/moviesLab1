using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Models.db;
using moviesAPI.Repositories;
using moviesAPI.Validators;
using NuGet.Protocol.Core.Types;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly CinemaRepository _repository;
        private readonly EntityValidator _validator;
        private readonly EntityExistsChecker _existsChecker;

        public MoviesController(CinemaRepository repository, EntityValidator validator, EntityExistsChecker checker)
        {
            _repository = repository;
            _validator = validator;
            _existsChecker = checker;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var entities = await _repository.GetMovies();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            var entity = await _repository.GetMovieById(Guid.Parse(id));
            if (entity == null)
            {
                return BadRequest();
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> PutMovie(string id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("delete-by-id")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateMovie(Movie movie)
        {
            if (_context.Movies == null) 
                return BadRequest("Entity set 'context.Movies' is null.");
            if (isMovieInvalid(movie) != "") 
                return BadRequest(isMovieInvalid(movie));
            if (MovieExists(movie.Id))
                return BadRequest($"movie with id '{movie.Id}' already exists");

            _context.Movies.Add(movie);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost("date")]
        public ActionResult MoviesThisDay(DateOnly date)
        {
            return Ok(_filterService.getMoviesByDay(_context, date));
        }
        public struct dateIntervalModel
        {
            public DateOnly dateFrom { get; set; }
            public DateOnly dateTo { get; set; }
        }
        [HttpPost("date-interval")]
        public ActionResult MoviesThisInterval(dateIntervalModel interval)
        {
            DateOnly dateFrom = interval.dateFrom;
            DateOnly dateTo = interval.dateTo;
            return Ok(_filterService.getMoviesByDateInterval(_context,dateFrom, dateTo));
        }

        [HttpPost("genres")]
        public ActionResult MoviesByGenres(int?[] genresId)
        {
            var movies = new List<Movie>();
            foreach (var id in genresId)
            {
                var res = _filterService.getMoviesByGenres(_context, id);
                foreach (var m in res)
                {
                    if (!movies.Contains(m))
                        movies.Add(m);
                }
            }
            return Ok(movies);
        }
        private bool MovieExists(Guid id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
