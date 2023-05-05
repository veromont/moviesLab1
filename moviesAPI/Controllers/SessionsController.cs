using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Interfaces;
using moviesAPI.Models;
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

        [HttpGet("session")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            return await _context.Sessions.ToListAsync();
        }

        [HttpGet("sessions")]
        public async Task<ActionResult<Session>> GetSession(string id)
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return session;
        }

        [HttpPut]
        public async Task<IActionResult> PutSession(string id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Session>> PostSession(Session session)
        {
            if (_context.Sessions == null)
            {
                return Problem("Entity set 'MovieCinemaLabContext.Sessions'  is null.");
            }
            _context.Sessions.Add(session);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SessionExists(session.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSession", new { id = session.Id }, session);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSession(string id)
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(string id)
        {
            return (_context.Sessions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
