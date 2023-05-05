using moviesAPI.Filters.DateFilters;
using moviesAPI.Interfaces;
using moviesAPI.Models;
using moviesAPI.Models.dbContext;

namespace moviesAPI.Services
{
    public class MovieFilterService: IMovieFilterService
    {
        MovieDateFilter _dateFilter;
        MovieGenreFilter _genreFilter;
        public MovieFilterService()
        {
            _dateFilter = new MovieDateFilter();
            _genreFilter = new MovieGenreFilter();
        }

        public ICollection<Movie> getMoviesByDay(MovieCinemaLabContext context, DateOnly date)
        {
            var movies = getMovies(context);
            return _dateFilter.GetByDay(movies, date);
        }

        public ICollection<Movie> getMoviesByDateInterval(MovieCinemaLabContext context,
                                                          DateOnly dateFrom, DateOnly dateTo)
        {
            var movies = getMovies(context);
            return _dateFilter.GetByDateInterval(movies, dateFrom, dateTo);
        }

        public ICollection<Movie> getMoviesByGenres(MovieCinemaLabContext context, int genreId)
        {
            var movies = getMovies(context);
            return _genreFilter.getMoviesByGenre(movies, genreId);
        }

        private ICollection<Movie> getMovies(MovieCinemaLabContext context)
        {
            return context.Movies.ToArray();
        }
    }
}
