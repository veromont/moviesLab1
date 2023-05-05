using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using moviesAPI.Interfaces;
using moviesAPI.Models;
using moviesAPI.Models.dbContext;
using Newtonsoft.Json.Linq;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCinemaLabContext _context;
        private readonly IMovieFilterService _filterService;
        public MoviesController(MovieCinemaLabContext context, IMovieFilterService movieFilterService)
        {
            _context = context;
            _filterService = movieFilterService;
        }

        #region generated endpoints
        [HttpGet("get-movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            return await _context.Movies.ToListAsync();
        }

        [HttpGet("get-movie")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
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

            return movie;
        }

        [HttpPut]
        public async Task<IActionResult> PutMovie(string id, Movie movie)
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

        [HttpDelete]
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
        #endregion
        
        [HttpPost]
        public async Task<ActionResult> CreateMovie(Movie movie)
        {
            if (_context.Movies == null) 
                return BadRequest("Entity set 'context.Movies' is null.");
            if (isMovieInvalid(movie)) 
                return BadRequest("movie failed validation");
            if (MovieExists(movie.Id))
                return BadRequest($"movie with id '{movie.Id}' already exists");

            _context.Movies.Add(movie);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("unknown exception");
            }

            return Ok();
        }

        [HttpPost("date")]
        public ActionResult MoviesThisDay(DateOnly date)
        {
            return Ok(_filterService.getMoviesByDay(_context, date));
        }

        [HttpPost("dateInterval")]
        public ActionResult MoviesThisInterval([FromBody] JObject data)
        {
            DateOnly dateFrom = data["dateFrom"].ToObject<DateOnly>();
            DateOnly dateTo = data["dateTo"].ToObject<DateOnly>();
            return Ok(_filterService.getMoviesByDateInterval(_context,dateFrom, dateTo));
        }

        [HttpPost("genre")]
        public ActionResult MoviesThisGenre(int genreId)
        {
            return Ok(_filterService.getMoviesByGenres(_context, genreId));
        }
        private bool MovieExists(string id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool isMovieInvalid(Movie movie)
        {
            const int TOO_LONG = 8;
            const int TOO_SHORT = 1;
            const int START_OF_CINEMATOGRAPHY = 1890;
            const int MIN_RATING = 0;
            const int MAX_RATING = 10;

            var movieGenre = _context.Genres.Select(x => x).Where(genre => genre.Id == movie.GenreId).ToList();

            if (movie.Duration.Hour > TOO_LONG || movie.Duration.Hour < TOO_SHORT) return true;
            if (movie.ReleaseDate.Year <= START_OF_CINEMATOGRAPHY) return true;
            if (movie.Rating < MIN_RATING || movie.Rating > MAX_RATING) return true;
            if (movieGenre.IsNullOrEmpty() && movie.GenreId != null) return true;

            movie.Genre = movieGenre[0];

            return false;
        }
    }
}
