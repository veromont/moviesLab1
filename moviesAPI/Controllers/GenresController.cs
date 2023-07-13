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
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public GenresController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var entities = await _repository.Get<Genre>();
            if (entities == null)
            {
                return BadRequest("ніц не знайдено");
            }

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var entity = await _repository.GetById<Genre>(id);
            if (entity == null)
            {
                return BadRequest("ніц не знайдено");
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateGenre(int id, Genre genre)
        {
            if (id != genre.Id) 
                return BadRequest("вказано некоректний id");

            if (!await genreExists(id))
                return BadRequest($"Жанр з id {genre.Id} не існує");

            var validationResult = _validator.isGenreInvalid(genre);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(id, genre);
            if (!updatedSuccessfully)
                return BadRequest("Не вдалося оновити");

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest("Не вдалося зберегти");

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> InsertGenre(Genre genre)
        {
            if (await genreExists(genre.Id))
                return BadRequest($"Жанр з id {genre.Id} уже існує");

            var validationResult = _validator.isGenreInvalid(genre);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(genre);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteGenre(int id)
        {
            if (!await genreExists(id))
                return BadRequest($"Жанр з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Genre>(id);

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }
        private async Task<bool> genreExists(int id)
        {
            return await _repository.GetById<Genre>(id) != null;
        }
    }
}
