using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.db;

namespace moviesAPI.Repositories
{
    public partial class CinemaRepository
    {
        public async Task<bool> DeleteMovie(Guid id)
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            context.Movies.Remove(movie);
            return true;
        }
        public async Task<bool> DeleteHall(int id)
        {
            var hall = await context.Halls.FindAsync(id);
            if (hall == null)
                return false;

            context.Halls.Remove(hall);
            return true;
        }
        public async Task<bool> DeleteSession(Guid id)
        {
            var session = await context.Sessions.FindAsync(id);
            if (session == null)
                return false;

            context.Sessions.Remove(session);
            return true;
        }
        public async Task<bool> DeleteTicket(Guid id)
        {
            var ticket = await context.Tickets.FindAsync(id);
            if (ticket == null)
                return false;

            context.Tickets.Remove(ticket);
            return true;
        }
        public async Task<bool> DeleteGenre(int id)
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre == null)
                return false;

            context.Genres.Remove(genre);
            return true;
        }
    }
}
