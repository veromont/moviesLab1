using moviesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.FileTransform;
using moviesAPI.Models.db;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        private readonly PdfTransform _pdfTransformer;
        public TicketsController(GenericCinemaRepository repository, EntityValidator validator, PdfTransform pdfTransformer)
        {
            _repository = repository;
            _validator = validator;
            _pdfTransformer = pdfTransformer;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetSessions()
        {
            var entities = await _repository.Get<Ticket>();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateTickets(string id, Session ticket)
        {
            var uuid = Guid.Parse(id);
            if (uuid != ticket.Id)
                return BadRequest();

            if (!await ticketExists(ticket.Id.ToString()))
                return BadRequest($"Квитка з id {ticket.Id} не існує");

            var validationResult = await _validator.isSessionInvalid(ticket);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(uuid, ticket);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> InsertTicket(Ticket ticket)
        {
            if (await ticketExists(ticket.Id.ToString()))
                return BadRequest($"Квиток з id {ticket.Id} уже існує");

            var validationResult = await _validator.isTicketInvalid(ticket);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(ticket);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteTicket(string id)
        {
            if (!await ticketExists(id))
                return BadRequest($"Квитка з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Ticket>(Guid.Parse(id));

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("get-pdf")]
        public async Task<ActionResult<FileStream>> GetTicketAsPDF(string ticketId)
        {
            var uuid = Guid.Parse(ticketId);
            var fileName = "ticket.pdf";
            var contentType = "application/pdf";

            var ticket = await _repository.GetById<Ticket>(uuid);
            if (ticket == null)
            {
                return BadRequest(); 
            }

            var session = ticket.Session;
            if (session == null)
            {
                return BadRequest();
            }

            var movie = await _repository.GetById<Movie>(session.MovieId);
            if (movie == null)
            {
                return BadRequest();
            }

            var hall = await _repository.GetById<Hall>(session.HallId);
            if (hall == null)
            {
                return BadRequest();
            }
            var ticketInfo = new PdfTicketModel(ticket, movie.Title, session.StartTime.ToString(), session.EndTime.ToString(), hall.Name);
            var memoryStream = _pdfTransformer.TransformTicketToPdf(ticketInfo);
            return File(memoryStream, contentType, fileName);
        }

        private Task<bool> ticketExists(string id)
        {
            return _repository.EntityExists<Movie>(Guid.Parse(id));
        }
    }
}
