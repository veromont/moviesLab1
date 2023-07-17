using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;

namespace moviesAPI.Models.ViewModels
{
    public class GenreModel : PageModel
    {
        private readonly GenericCinemaRepository _repository;
        public GenreModel(GenericCinemaRepository repository)
        {
            _repository = repository;
            NewGenre = new Genre();
        }

        [BindProperty]
        public Genre NewGenre { get; set; }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            return RedirectToPage();
        }
    }
}
