﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models;
using moviesAPI.Models.db;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;

        public MoviesController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var entities = await _repository.Get<Movie>();
            if (entities == null)
                return BadRequest();

            return Ok(entities);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<Movie>> GetMovie(string id)
        {
            var entity = await _repository.GetById<Movie>(Guid.Parse(id));
            if (entity == null)
            {
                return BadRequest();
            }

            return Ok(entity);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateMovie(string id, Movie movie)
        {
            var uuid = Guid.Parse(id); // TODO: uuid parsing global method
            if (uuid != movie.Id)
                return BadRequest();

            if (!await movieExists(movie.Id.ToString()))
                return BadRequest($"Фільм з id {movie.Id} не існує");

            var validationResult = await _validator.isMovieInvalid(movie);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            var updatedSuccessfully = await _repository.Update(uuid, movie);
            if (!updatedSuccessfully)
                return BadRequest();

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult> DeleteMovie(string id)
        {
            if (!await movieExists(id))
                return BadRequest($"Фільм з id {id} не існує");

            var deletedSuccessfully = await _repository.Delete<Movie>(Guid.Parse(id));

            if (deletedSuccessfully)
            {
                var savedSuccessfully = await _repository.Save();
                if (!savedSuccessfully)
                    return BadRequest();

                return Ok();
            }

            return BadRequest();
        }
        
        [HttpPost("insert")]
        public async Task<ActionResult> InsertMovie(Movie movie)
        {
            if (await movieExists(movie.Id.ToString()))
                return BadRequest($"Фільм з id {movie.Id} уже існує");

            var validationResult = await _validator.isMovieInvalid(movie);
            if (validationResult != string.Empty)
                return BadRequest(validationResult);

            await _repository.Insert(movie);

            var savedSuccessfully = await _repository.Save();
            if (!savedSuccessfully)
                return BadRequest();

            return Ok();
        }
        [HttpPost("get-by-date")]
        public async Task<ActionResult<IEnumerable<Movie>>> MoviesThisDay(DateOnly date)
        {
            var movies = await _repository.Get<Movie>();
            var result = from m in movies
                         where m.ReleaseDate.Day == date.Day && m.ReleaseDate.Month == date.Month
                         select m;
            return result.ToArray();
        }

        [HttpPost("get-by-date-interval")]
        public async Task<ActionResult<IEnumerable<Movie>>> MoviesThisInterval(DateOnlyIntervalModel interval)
        {
            DateOnly dateFrom = interval.dateFrom;
            DateOnly dateTo = interval.dateTo;
            var movies = await _repository.Get<Movie>();
            var result = from m in movies
                         where m.ReleaseDate >= dateFrom && m.ReleaseDate <= dateTo
                         select m;
            return result.ToArray();
        }

        [HttpPost("get-by-genres")]
        public async Task<ActionResult<IEnumerable<Movie>>> MoviesByGenres(int?[] genresIds)
        {
            var movies = await _repository.Get<Movie>();
            var result = from m in movies
                         where genresIds.Contains(m.GenreId)
                         select m;
            return result.ToArray();
        }
        private Task<bool> movieExists(string id)
        {
            return _repository.EntityExists<Movie>(Guid.Parse(id));
        }
    }
}
