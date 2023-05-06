using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using moviesAPI.Interfaces;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;
using moviesAPI.Services;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly MovieCinemaLabContext _context;
        private readonly ISessionFilterService _filterService;
        public SessionsController(MovieCinemaLabContext context, ISessionFilterService filterService)
        {
            _context = context;
            _filterService = filterService;
        }

        [HttpGet("get-sessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            if (_context.Sessions == null) return NotFound();
            return await _context.Sessions.ToListAsync();
        }

        [HttpGet("get-session")]
        public async Task<ActionResult<Session>> GetSession(string id)
        {
            if (_context.Sessions == null) return NotFound();
            var session = await _context.Sessions.FindAsync(id);

            if (session == null) return NotFound();

            return session;
        }

        [HttpPut]
        public async Task<IActionResult> PutSession(string id, Session session)
        {
            if (id != session.Id) return BadRequest();
            if (!SessionExists(id)) return NotFound();
            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostSession(Session session)
        {
            if (_context.Sessions == null) return Problem("Entity set 'context.Sessions' is null.");
            if (SessionExists(session.Id)) return BadRequest($"session with id {session.Id} already exists");
            if (isSessionInvalid(session)) return BadRequest("session failed validation");

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

        private bool SessionExists(string id)
        {
            return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool isSessionInvalid(Session session)
        {
            var movie = _context.Movies.Find(session.MovieId);
            var hall = _context.Halls.Find(session.HallId);
            if (movie == null || hall == null) return true;
            if (!hall.IsAvailable) return true;

            var sessionDuration = TimeOnly.FromTimeSpan(session.EndTime - session.StartTime);
            if (sessionDuration < movie.Duration) return true;
            
            var overlayingSessions = from s in _context.Sessions 
                                     where ((s.StartTime > session.StartTime && s.StartTime < session.EndTime) 
                                           || (s.EndTime > session.StartTime && s.EndTime < session.EndTime)
                                           || (s.StartTime < session.StartTime && s.EndTime > session.EndTime))
                                           && s.HallId == session.HallId
                                     select s;
            if (!overlayingSessions.IsNullOrEmpty()) return true;
            

            return false;
        }
    }
}