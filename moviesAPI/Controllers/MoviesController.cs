using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    public class MoviesController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public MoviesController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var entities = await _repository.GetAll<Movie>();

            return entities != null ?
                          View(entities) :
                          Problem("Нічого не знайдено");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _repository.GetById<Movie>(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        public async Task<IActionResult> Create()
        {
            var genres = await _repository.GetAll<Genre>();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie model)
        {
            var validationResult = await _validator.isMovieInvalid(model);
            model.Id = Guid.NewGuid();
            var genres = await _repository.GetAll<Genre>();
            ViewBag.Genres = new SelectList(genres, "Id", "Name");

            if (validationResult.Count > 0)
            {
                foreach (var errorMessage in validationResult)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
            }
            else
            {
                await _repository.Insert<Movie>(model);
                var saved = await _repository.Save();
                if (saved != string.Empty)
                {
                    ModelState.AddModelError("",saved);
                    return View(model);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _repository.GetById<Movie>(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(id, movie);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await movieExists(movie.Id))
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
            return View(movie);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _repository.GetById<Movie>(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.Delete<Movie>(id);
            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }
        private Task<bool> movieExists(Guid id)
        {
            return _repository.EntityExists<Movie>(id);
        }
    }
}
