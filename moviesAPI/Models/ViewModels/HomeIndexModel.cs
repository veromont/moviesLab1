using moviesAPI.Models.EntityModels;

namespace moviesAPI.Models.ViewModels
{
    public class HomeIndexModel
    {
        public IEnumerable<Movie> Movies { get; set; }
    }
}
