using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    public class HallsController : Controller
    {
		private readonly GenericCinemaRepository _repository;
		private readonly EntityValidator _validator;
		public HallsController(GenericCinemaRepository repository, EntityValidator validator)
		{
			_repository = repository;
			_validator = validator;
		}

		public async Task<IActionResult> Index()
        {
			var entities = await _repository.GetAll<Hall>();

            return entities != null ?
                          View(entities) :
                          Problem("Нічого не знайдено");
        }
        public async Task<IActionResult> Details(int id)
        {
			var entity = await _repository.GetById<Hall>(id);
			if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity,IsAvailable")] Hall hall)
        {
			var validationResult = _validator.isHallInvalid(hall);

			if (validationResult.Count > 0)
			{
				foreach (var errorMessage in validationResult)
				{
					ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
				}
			}
			else
            {
                await _repository.Insert(hall);
                await _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(hall);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _repository.GetById<Hall>(id);
            if (hall == null)
            {
                return NotFound();
            }
            return View(hall);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity,IsAvailable")] Hall hall)
        {
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(id ,hall);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await hallExists(hall.Id))
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
            return View(hall);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _repository.GetById<Hall>(id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.Delete<Hall>(id);
            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }
		private Task<bool> hallExists(int id)
		{
			return _repository.EntityExists<Hall>(id);
		}
	}
}
