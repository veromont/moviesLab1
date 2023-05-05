using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("getMovies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            return await _context.Movies.ToListAsync();
        }

        [HttpGet("getMovie")]
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
        public async Task<string> CreateMovie(Movie movie)
        {
            if (_context.Movies == null)
            {
                return "Entity set 'MovieCinemaLabContext.Movies'  is null.";
            }
            if (movie.Genre == null)
            {
                var genre = (from gen in _context.Genres where gen.Id == movie.GenreId select gen).ToArray()[0];
                if (genre == null) return $"no genre with id {movie.GenreId}";
                movie.Genre = genre;
            }
            _context.Movies.Add(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MovieExists(movie.Id))
                {
                    return $"movie with id '{movie.Id}' already exists";
                }
                else
                {
                    return "unknown exception";
                }
            }

            return "Ok";
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
    }
}
