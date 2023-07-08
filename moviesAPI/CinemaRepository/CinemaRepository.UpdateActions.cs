using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.db;

namespace moviesAPI.Repositories
{
    public partial class CinemaRepository
    {
        public async Task<bool> UpdateMovie(Guid id,Movie movie)
        {
            var modifiedEntity = await context.Movies.FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(movie).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> UpdateHall(int id, Hall hall)
        {
            var modifiedEntity = await context.Halls.FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(hall).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> UpdateSession(Guid id, Session session)
        {
            var modifiedEntity = await context.Sessions.FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(session).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> UpdateTicket(Guid id, Ticket ticket)
        {
            var modifiedEntity = await context.Tickets.FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(ticket).State = EntityState.Modified;
            return true;
        }
        public async Task<bool> UpdateGenre(int id, Genre genre)
        {
            var modifiedEntity = await context.Genres.FindAsync(id);
            if (modifiedEntity == null)
                return false;

            context.Entry(genre).State = EntityState.Modified;
            return true;
        }
    }
}
