using moviesAPI.Interfaces;
using moviesAPI.Models;

namespace moviesAPI.Filters.DateFilters
{
    public class SessionDateFilter
    {
        public ICollection<Session> GetByDateInterval(ICollection<Session> sessions, DateTime dateFrom, DateTime dateTo)
        {
            var result = from s in sessions
                         where s.StartTime <= dateTo && s.StartTime >= dateFrom
                         select s;
            return result.ToArray();
        }

        public ICollection<Session> GetByDay(ICollection<Session> sessions, DateOnly date)
        {
            var result = from s in sessions
                         where s.StartTime.Year == date.Year && s.StartTime.Month == date.Month && s.StartTime.Day == date.Day
                         select s;
            return result.ToArray();
        }
    }
}
