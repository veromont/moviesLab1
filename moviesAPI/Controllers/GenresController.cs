﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.EntityModels;
using moviesAPI.Models.ViewModels;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    public class GenresController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public GenresController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<IActionResult> Index()
        {
            var entities = await _repository.GetAll<Genre>();

            return entities != null ?
                          View(entities) :
                          Problem("Нічого не знайдено");
        }
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _repository.GetById<Genre>(id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new GenreViewModel(entity);
            if (model.MovieWithThisGenreCount > 0)
            {
                return View(model);
            }

            var movies = await _repository.GetAll<Movie>();
            model.MovieWithThisGenreCount = movies.Where(m => m.GenreId == entity.Id).Count();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre model)
        {
            var validationResult = _validator.isGenreInvalid(model);
            var sameIdEntity = await _repository.GetById<Genre>(model.Id);

            if (validationResult.Count > 0)
            {
                foreach (var errorMessage in validationResult)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
                return View(model);
            }
            else if (sameIdEntity is not null)
            {
                ModelState.AddModelError(nameof(Genre.Id), "Такий id існує");
                return View(model);
            }
            else
            {
                await _repository.Insert(model);
                await _repository.Save();
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _repository.GetById<Genre>(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(id, genre);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await genreExists(genre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _repository.GetById<Genre>(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete<Genre>(id);
            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }
        private Task<bool> genreExists(int id)
        {
            return _repository.EntityExists<Genre>(id);
        }
    }
}
