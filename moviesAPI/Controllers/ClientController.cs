using Microsoft.AspNetCore.Mvc;
using moviesAPI.Repositories;
using moviesAPI.Models.db;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public ClientsController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var entities = await _repository.Get<Client>();
            if (entities == null)
            {
                return BadRequest("ніц не знайдено");
            }

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var entity = await _repository.GetById<Client>(id);
            if (entity == null)
            {
                return BadRequest("ніц не знайдено");
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateClient(string id, Client client)
        {
            if (!Guid.TryParse(id, out var uuid))
                return BadRequest("Некоректний формат id");

            if (uuid != client.Id)
                return BadRequest("вказано некоректний uuid");

            if (!await clientExists(uuid))
                return BadRequest($"Клієнта з id {client.Id} не існує");

            var validationResult = await _validator.isClientInvalid(client);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(uuid, client);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> InsertClient(Client client)
        {
            if (await clientExists(client.Id))
                return BadRequest($"Клієнт з id {client.Id} уже існує");

            var validationResult = await _validator.isClientInvalid(client);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(client);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteClient(string id)
        {
            if (!Guid.TryParse(id, out var uuid))
                return BadRequest("Некоректний формат id");

            if (!await clientExists(uuid))
                return BadRequest($"Клієнта з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Client>(uuid);

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }
        private async Task<bool> clientExists(Guid id)
        {
            return await _repository.GetById<Client>(id) != null;
        }
    }
}

