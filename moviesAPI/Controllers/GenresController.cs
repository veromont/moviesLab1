using Microsoft.AspNetCore.Mvc;
using moviesAPI.Repositories;
using moviesAPI.Models.db;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly CinemaRepository _repository;
        private readonly EntityValidator _validator;
        private readonly EntityExistsChecker _existsChecker;
        public GenresController(CinemaRepository repository, EntityValidator validator, EntityExistsChecker checker)
        {
            _repository = repository;
            _validator = validator;
            _existsChecker = checker;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var entities = await _repository.GetGenres();
            if (entities == null)
            {
                return BadRequest();
            }

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var entity = await _repository.GetGenreById(id);
            if (entity == null)
            {
                return BadRequest();
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateGenre(int id, Genre genre)
        {
            if (id != genre.Id || ! await _existsChecker.GenreExists(id)) 
                return BadRequest();

            var validationResult = _validator.isGenreInvalid(genre);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.UpdateGenre(id, genre);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> PostGenre(Genre genre)
        {
            var validationResult = _validator.isGenreInvalid(genre);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.InsertGenre(genre);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            var deletedSuccessfully = await _repository.DeleteGenre(id);

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }
    }
}
