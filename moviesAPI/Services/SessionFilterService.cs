using moviesAPI.Filters.DateFilters;
using moviesAPI.Interfaces;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;

namespace moviesAPI.Services
{
    public class SessionFilterService: ISessionFilterService
    {
        SessionDateFilter _dateFilter;

        public SessionFilterService() 
        {
            _dateFilter = new SessionDateFilter();
        }
       public ICollection<Session> getSessionsByDay(MovieCinemaLabContext context, DateOnly date)
        {
            var sessions = context.Sessions.ToArray();
            return _dateFilter.GetByDay(sessions,date);
        }
        public ICollection<Session> getSessionsByDateInterval(MovieCinemaLabContext context, DateTime dateFrom, DateTime dateTo)
        {
            var sessions = context.Sessions.ToArray();
            return _dateFilter.GetByDateInterval(sessions, dateFrom, dateTo);
        }
    }
}
