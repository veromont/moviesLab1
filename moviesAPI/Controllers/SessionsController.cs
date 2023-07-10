using Microsoft.AspNetCore.Mvc;
using moviesAPI.Models;
using moviesAPI.Models.db;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;

        public SessionsController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("get-served-sessions")]
        public async Task<ActionResult<IEnumerable<ServedSession>>> GetServedSessions()
        {
            var sessions = await _repository.Get<Session>();
            var res = new List<ServedSession>();

            foreach (var session in sessions) 
            {
                var movie = await _repository.GetById<Movie>(session.MovieId);
                var hall = await _repository.GetById<Hall>(session.HallId);
                if (movie == null || hall == null)
                {
                    continue;
                }
                ServedSession temp = new ServedSession(session, movie.Title, hall.Name);
                res.Add(temp);
            }
            return res;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            var entities = await _repository.Get<Session>();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateSession(string id, Session session)
        {
            var uuid = Guid.Parse(id);
            if (uuid != session.Id)
                return BadRequest();

            if (!await sessionExists(session.Id.ToString()))
                return BadRequest($"Сеансу з id {session.Id} не існує");

            var validationResult = await _validator.isSessionInvalid(session);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(uuid, session);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> InsertSession(Session session)
        {
            if (await sessionExists(session.Id.ToString()))
                return BadRequest($"Сеанс з id {session.Id} уже існує");

            var validationResult = await _validator.isSessionInvalid(session);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(session);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteSession(string id)
        {
            if (!await sessionExists(id))
                return BadRequest($"Сеансу з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Session>(Guid.Parse(id));

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("get-by-date")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByDate(DateOnly date)
        {
            var sessions = await _repository.Get<Session>();
            var result = from s in sessions
                         where s.StartTime.Year == date.Year && s.StartTime.Month == date.Month && s.StartTime.Day == date.Day
                         select s;
            return result.ToArray();
        }

        [HttpPost("get-by-date-interval")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByDateInterval(DateTime dateFrom, DateTime dateTo)
        {
            var sessions = await _repository.Get<Session>();
            var result = from s in sessions
                         where s.StartTime <= dateTo && s.StartTime >= dateFrom
                         select s;
            return result.ToArray();
        }

        [HttpPost("get-by-movieId")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByMovie(string movieId)
        {
            var sessions = await _repository.Get<Session>();
            var result = from s in sessions
                         where s.MovieId.ToString() == movieId
                         select s;
            return result.ToArray();
        }
        private Task<bool> sessionExists(string id)
        {
            return _repository.EntityExists<Session>(Guid.Parse(id));
        }
    }
}