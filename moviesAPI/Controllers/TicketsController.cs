using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.dbContext;
using moviesAPI.FileTransform;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Org.BouncyCastle.Utilities;
using Microsoft.IdentityModel.Tokens;
using moviesAPI.Models.db;
using moviesAPI.Models;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly MovieCinemaLabContext _context;
        private readonly PdfTransform pdfTransform;
        private readonly TicketInfoModelConstructor ticketModelConstructor;
        public TicketsController(MovieCinemaLabContext context)
        {
            _context = context;
            pdfTransform = new PdfTransform();
            ticketModelConstructor = new TicketInfoModelConstructor(context);
        }

        [HttpGet("get-tickets")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            if (_context.Tickets == null) return NotFound();
            return await _context.Tickets.ToListAsync();
        }

        [HttpGet("get-ticket")]
        public async Task<ActionResult<Ticket>> GetTicket(string id)
        {
            if (_context.Tickets == null)
                return NotFound();
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound();

            return ticket;
        }

        [HttpPut]
        public async Task<IActionResult> PutTicket(string id, Ticket ticket)
        {
            if (id != ticket.Id) return BadRequest();
            if (!TicketExists(id)) return NotFound();

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> PostTicket(Ticket ticket)
        {
            if (_context.Tickets == null)
                return BadRequest("Entity set 'context.Tickets' is null.");
            if (TicketExists(ticket.Id))
                return BadRequest($"ticket with id {ticket.Id} exists");
            if (isTicketInvalid(ticket))
                return BadRequest($"ticket failed validation");

            _context.Tickets.Add(ticket);

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
        public async Task<IActionResult> DeleteTicket(string id)
        {
            if (_context.Tickets == null) return NotFound();
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("get-pdf-ticket")]
        public FileStreamResult GetTicketAsPDF(string ticketId)
        {
            var fileName = "ticket.pdf";
            var contentType = "application/pdf";
            var t = _context.Tickets.Find(ticketId);
            var ticketInfo = ticketModelConstructor.getpdfTicketModel(t);
            var memoryStream = pdfTransform.TransformTicketToPdf(ticketInfo);
            return File(memoryStream, contentType, fileName);
        }

        private bool TicketExists(string id)
        {
            return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool isTicketInvalid(Ticket ticket)
        {
            var sessions = _context.Sessions.Select(x => x).Where(x => x.Id == ticket.SessionId).ToArray();
            if (sessions.IsNullOrEmpty()) return true;
            var session = sessions[0];
            var hall = _context.Halls.Find(session.HallId);
            if (session.SessionTickets != null)
            {
                var sameSeatTickets = session.SessionTickets.Select(x => x)
                                                            .Where(t => t.SeatNumber == ticket.SeatNumber);
                if (sameSeatTickets.Count() > 0) return true;
            }
            if (hall.Capacity < ticket.SeatNumber || ticket.SeatNumber <= 0) return true;
            if (!hall.IsAvailable) return true;
            if (ticket.Price < 0) return true;
            return false;
        }
    }
}
