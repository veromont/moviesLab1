using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Repositories;
using moviesAPI.Models.db;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallsController : ControllerBase
    {
        private readonly CinemaRepository _repository;
        private readonly EntityValidator _validator;
        private readonly EntityExistsChecker _existsChecker; //TODO: implement
        public HallsController(CinemaRepository repository, EntityValidator validator, EntityExistsChecker checker)
        {
            _repository = repository;
            _validator = validator;
            _existsChecker = checker;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Hall>>> GetHalls()
        {
            var entities = await _repository.GetHalls();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Hall>> GetHall(int id)
        {
            var entity = await _repository.GetHallById(id);
            if (entity == null)
            {
                return BadRequest();
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateHall(int id, Hall hall)
        {
            if (id != hall.Id)
                return BadRequest();

            var validationResult = _validator.isHallInvalid(hall);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.UpdateHall(id, hall);
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
            var validationResult = _validator.isHallInvalid(hall);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.InsertHall(hall);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteHall(int id)
        {
            var deletedSuccessfully = await _repository.DeleteHall(id);

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
