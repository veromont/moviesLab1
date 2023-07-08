using moviesAPI.Filters.DateFilters;
using moviesAPI.Interfaces;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Models.db;
using NPOI.OpenXmlFormats.Dml;

namespace moviesAPI.Services
{
    public class SessionFilterService: ISessionFilterService
    {
        SessionDateFilter _dateFilter;

        public SessionFilterService() 
        {
            _dateFilter = new SessionDateFilter();
        }
       public ICollection<Session> getSessionsByDay(CinemaContext context, DateOnly date)
        {
            var sessions = context.Sessions.ToArray();
            return _dateFilter.GetByDay(sessions,date);
        }
        public ICollection<Session> getSessionsByDateInterval(CinemaContext context, DateTime dateFrom, DateTime dateTo)
        {
            var sessions = context.Sessions.ToArray();
            return _dateFilter.GetByDateInterval(sessions, dateFrom, dateTo);
        }

        public ICollection<Session> getSessionsByMovie(CinemaContext context, string movieId)
        {
            var movie = context.Movies.Find(movieId);
            var sessions = context.Sessions;
            if (movie == null) return new List<Session>();
            var result = from s in sessions 
                         where s.MovieId == movieId 
                         select s;
            return result.ToArray();
        }
    }
}
