using moviesAPI.Models;
using moviesAPI.Models.dbContext;

namespace moviesAPI.Interfaces
{
    public interface IMovieFilterService
    {
        public ICollection<Movie> getMoviesByDay(MovieCinemaLabContext context, DateOnly date);
        public ICollection<Movie> getMoviesByDateInterval(MovieCinemaLabContext context, 
                                                          DateOnly dateFrom, DateOnly dateTo);
        public ICollection<Movie> getMoviesByGenres(MovieCinemaLabContext context, int genreId);
    }
}
