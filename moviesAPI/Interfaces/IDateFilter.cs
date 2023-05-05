using moviesAPI.Models.dbContext;

namespace moviesAPI.Interfaces
{
    public interface IDateFilter
    {
        object GetByDay(ICollection<object> context);
        ICollection<object> GetByDateInterval(ICollection<object> context);
    }
}
