using moviesAPI.Models.db;

namespace moviesAPI.Repositories
{
    public partial class CinemaRepository
    {
        public async Task<bool> InsertMovie(Movie movie)
        {
            await context.Movies.AddAsync(movie);
            return true;
        }
        public async Task<bool> InsertHall(Hall hall)
        {
            await context.Halls.AddAsync(hall);
            return true;
        }
        public async Task<bool> InsertSession(Session session)
        {
            await context.Sessions.AddAsync(session);
            return true;
        }
        public async Task<bool> InsertTicket(Ticket ticket)
        {
            await context.Tickets.AddAsync(ticket);
            return true;
        }
        public async Task<bool> InsertGenre(Genre genre)
        {
            await context.Genres.AddAsync(genre);
            return true;
        }
    }
}
