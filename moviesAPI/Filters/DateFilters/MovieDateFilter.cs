using moviesAPI.Interfaces;
using moviesAPI.Models.db;

namespace moviesAPI.Filters.DateFilters
{
    public class MovieDateFilter
    {
        public ICollection<Movie> GetByDateInterval(ICollection<Movie> movies, DateOnly dateFrom, DateOnly dateTo)
        {
            var result = from m in movies 
                         where m.ReleaseDate >= dateFrom && m.ReleaseDate <= dateTo 
                         select m;
            return result.ToArray();
        }

        public ICollection<Movie> GetByDay(ICollection<Movie> movies, DateOnly date)
        {
            var result = from m in movies 
                         where m.ReleaseDate.Day == date.Day && m.ReleaseDate.Month == date.Month 
                         select m;
            return result.ToArray();
        }
    }
}
