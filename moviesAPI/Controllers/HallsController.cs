using Microsoft.AspNetCore.Mvc;
using moviesAPI.Repositories;
using moviesAPI.Models.db;
using moviesAPI.Validators;
using System;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public HallsController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            var entities = await _repository.Get<Hall>();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Hall>> GetHall(int id)
        {
            var entity = await _repository.GetById<Hall>(id);
            if (entity == null)
            {
                return BadRequest();
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateHall(int id, Hall hall)
        {
            if (!await hallExists(id))
                return BadRequest($"Зал з id {hall.Id} не існує");

            if (id != hall.Id)
                return BadRequest();

            var validationResult = _validator.isHallInvalid(hall);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(id, hall);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpPost("insert")]
        public async Task<ActionResult> PostHall(Hall hall)
        {
            if (await hallExists(hall.Id))
                return BadRequest($"Зал з id {hall.Id} уже існує");

            var validationResult = _validator.isHallInvalid(hall);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(hall);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteHall(int id)
        {
            if (!await hallExists(id))
                return BadRequest($"Зал з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Hall>(id);

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }
        private Task<bool> hallExists(int id)
        {
            return _repository.EntityExists<Hall>(id);
        }
    }
}
