using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly MovieCinemaLabContext _context;

        public HallsController(MovieCinemaLabContext context)
        {
            _context = context;
        }

        [HttpGet("get-halls")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            if (_context.Halls == null) return NotFound();
            return await _context.Halls.ToListAsync();
        }

        [HttpGet("get-hall")]
        public async Task<ActionResult<Hall>> GetHall(int id)
        {
            if (_context.Halls == null) return NotFound();
            var hall = await _context.Halls.FindAsync(id);

            if (hall == null) return NotFound();

            return hall;
        }

        [HttpPut]
        public async Task<IActionResult> PutHall(int id, Hall hall)
        {
            if (id != hall.Id) return BadRequest();
            if (!HallExists(id)) return NotFound();
            _context.Entry(hall).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostHall(Hall hall)
        {
            if (_context.Halls == null) return BadRequest("Entity set 'context.Halls' is null.");
            if (HallExists(hall.Id)) return BadRequest($"hall with id {hall.Id} already exists");
            if (isHallInvalid(hall)) return BadRequest("hall failed validation");

            _context.Halls.Add(hall);

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
        public async Task<IActionResult> DeleteHall(int id)
        {
            if (_context.Halls == null) return NotFound();
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null) return NotFound();
            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HallExists(int id)
        {
            return (_context.Halls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        bool isHallInvalid(Hall hall)
        {
            const int MAX_CAPACITY = 10000;
            const int MIN_CAPACITY = 0;
            if (hall.Capacity <= MIN_CAPACITY || hall.Capacity >= MAX_CAPACITY) return true;
            return false;
        }
    }
}
