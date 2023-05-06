using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;

namespace moviesAPI.Interfaces
{
    public interface ISessionFilterService
    {
        public ICollection<Session> getSessionsByDay(MovieCinemaLabContext context, DateOnly date);
        public ICollection<Session> getSessionsByDateInterval(MovieCinemaLabContext context,
                                                          DateTime dateFrom, DateTime dateTo);
    }
}
