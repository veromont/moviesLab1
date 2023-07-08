using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Interfaces;
using moviesAPI.Models;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Models.db;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly ISessionFilterService _filterService;
        public SessionsController(CinemaContext context, ISessionFilterService filterService)
        {
            _context = context;
            _filterService = filterService;
        }

        [HttpGet("get-served-sessions")]
        public async Task<ActionResult<IEnumerable<ServedSession>>> GetServedSessions()
        {
            if (_context.Sessions == null) return NotFound();
            var res = new List<ServedSession>();
            foreach (var session in _context.Sessions) 
            {
                var movieName = _context.Movies.Find(session.MovieId).Title;
                var hallName = _context.Halls.Find(session.HallId).Name;
                ServedSession temp = new ServedSession(session,movieName,hallName);
                res.Add(temp);
            }
            return res;
        }

        [HttpGet("get-sessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            if (_context.Sessions == null) return NotFound();
            return await _context.Sessions.ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> PutSession(string id, Session session)
        {
            if (id != session.Id) return BadRequest();
            if (!SessionExists(id)) return NotFound();
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostSession(Session session)
        {
            if (_context.Sessions == null) return Problem("Entity set 'context.Sessions' is null.");
            if (SessionExists(session.Id)) return BadRequest($"session with id {session.Id} already exists");
            try
            {
                if (isSessionInvalid(session) != "") return BadRequest(isSessionInvalid(session));
            }
            catch
            {
                return BadRequest("неочікувана помилка, скоріше за все некоректні дати");
            }

            _context.Sessions.Add(session);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSession(string id)
        {
            if (_context.Sessions == null) return NotFound();
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            _context.Sessions.Remove(session);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("get-by-date")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByDate(DateOnly date)
        {
            return Ok(_filterService.getSessionsByDay(_context, date));
        }

        [HttpPost("get-by-date-interval")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByDateInterval(DateTime dateFrom, DateTime dateTo)
        {
            return Ok(_filterService.getSessionsByDateInterval(_context, dateFrom, dateTo));
        }

        [HttpPost("get-by-movieId")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByMovie(string movieId)
        {
            return Ok(_filterService.getSessionsByMovie(_context, movieId));
        }
        private bool SessionExists(string id)
        {
            return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}