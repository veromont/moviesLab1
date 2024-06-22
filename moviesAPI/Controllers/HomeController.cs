using Microsoft.AspNetCore.Mvc;
using moviesAPI.Models.EntityModels;
using moviesAPI.Models.ViewModels;
using moviesAPI.Repositories;

namespace moviesAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly GenericCinemaRepository _repository;
        public HomeController(GenericCinemaRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexModel();
            model.Movies = await _repository.GetAll<Movie>();
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
