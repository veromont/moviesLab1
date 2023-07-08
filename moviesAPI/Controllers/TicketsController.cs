using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.FileTransform;
using moviesAPI.Models.db;
using moviesAPI.Models.CinemaContext;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly CinemaContext _context;
        private readonly PdfTransform pdfTransform;
        private readonly TicketInfoModelConstructor ticketModelConstructor;
        public TicketsController(CinemaContext context)
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

        [HttpPost("create-ticket")]
        public async Task<ActionResult> PostTicket(Ticket ticket)
        {
            if (ticket.Id == null)
                ticket.Id = Guid.NewGuid().ToString();
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
        [HttpPost("book-ticket")]
        public async Task<ActionResult> BookTicket(string sessionId)
        {
            const int MAX_HALL_SIZE = 1000;
            const int DEFAULT_TICKET_PRICE = 100;
            if (_context.Tickets == null)
                return BadRequest("Entity set 'context.Tickets' is null.");

            var session = _context.Sessions.Find(sessionId);
            var bookedTickets = (from t in _context.Tickets where t.SessionId == sessionId select t).ToList();
            var ticket = new Ticket();
            ticket.Id = Guid.NewGuid().ToString();
            for (int i = 1; i < MAX_HALL_SIZE; i++)
            {
                var tickets = from t in bookedTickets where t.SeatNumber == i select t;
                if (tickets.Count() == 0)
                {
                    ticket.SeatNumber = i;
                    break;
                }
            }
            ticket.Price = DEFAULT_TICKET_PRICE;
            ticket.SessionId = sessionId;

            _context.Tickets.Add(ticket);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(ticket.Id);
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

        [HttpPost("get-pdf-ticket")]
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
    }
}
