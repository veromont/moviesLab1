using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;
using moviesAPI.Validators;

namespace moviesAPI.Controllers
{
    public class TicketsController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        private readonly EntityValidator _validator;
        public TicketsController(GenericCinemaRepository repository, EntityValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            var entities = await _repository.GetAll<Ticket>();

            return entities != null ?
                          View(entities) :
                          Problem("Нічого не знайдено");
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _repository.GetById<Ticket>(id);
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
        public async Task<IActionResult> Create(Ticket ticket)
        {
            var validationResult = await _validator.isTicketInvalid(ticket);

            if (validationResult.Count > 0)
            {
                foreach (var errorMessage in validationResult)
                {
                    ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
                }
            }
            else
            {
                await _repository.Insert(ticket);
                await _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _repository.GetById<Ticket>(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Capacity,IsAvailable")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.Update(id, ticket);
                    await _repository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ticketExists(ticket.Id))
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
            return View(ticket);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _repository.GetById<Ticket>(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _repository.Delete<Ticket>(id);
            await _repository.Save();
            return RedirectToAction(nameof(Index));
        }
        private Task<bool> ticketExists(Guid id)
        {
            return _repository.EntityExists<Ticket>(id);
        }
    }
}
