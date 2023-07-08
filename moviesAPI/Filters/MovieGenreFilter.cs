using MathNet.Numerics.Distributions;
using moviesAPI.Models.db;

namespace moviesAPI.Filters
{
    public class MovieGenreFilter
    {
        public ICollection<Movie> getMoviesByGenre(ICollection<Movie> movies, int? genreId)
        {
            var result = from m in movies
                         where m.GenreId == genreId
                         select m;
            return result.ToArray();
        }
    }
}
