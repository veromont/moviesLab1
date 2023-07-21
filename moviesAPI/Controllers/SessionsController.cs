using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    public class SessionsController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public SessionsController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _repository.GetAll<Session>();

            return entities != null ?
                          View(entities) :
                          Problem("Нічого не знайдено");
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _repository.GetById<Session>(id);
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
        public async Task<IActionResult> Create(Session session)
        {
            var validationResult = await _validator.isSessionInvalid(session);

            if (validationResult.Count > 0)
            {
                foreach (var errorMessage in validationResult)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
            }
            else
            {
                await _repository.Insert(session);
                await _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _repository.GetById<Session>(id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(id, session);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await sessionExists(session.Id))
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
            return View(session);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _repository.GetById<Session>(id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.Delete<Session>(id);
            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }
        private Task<bool> sessionExists(Guid id)
        {
            return _repository.EntityExists<Session>(id);
        }
    }
}
