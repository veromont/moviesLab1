using Microsoft.EntityFrameworkCore;
using moviesAPI.Models.db;

namespace moviesAPI.Repositories
{
    public partial class CinemaRepository
    {
        public async Task<IEnumerable<Movie>> GetMovies() 
        { 
            return await context.Movies.ToListAsync(); 
        }
        public async Task<IEnumerable<Hall>> GetHalls()
        { 
            return await context.Halls.ToListAsync(); 
        }
        public async Task<IEnumerable<Session>> GetSessions()
        {
            return await context.Sessions.ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await context.Tickets.ToListAsync();
        }
        public async Task<IEnumerable<Genre>> GetGenres() 
        { 
            return await context.Genres.ToListAsync(); 
        }


        public async Task<Movie?> GetMovieById(Guid id)
        {
            return await context.Movies.FindAsync(id);
        }
        public async Task<Hall?> GetHallById(int id)
        {
            return await context.Halls.FindAsync(id);
        }
        public async Task<Session?> GetSessionById(Guid id)
        {
            return await context.Sessions.FindAsync(id);
        }
        public async Task<Ticket?> GetTicketById(Guid id)
        {
            return await context.Tickets.FindAsync(id);
        }
        public async Task<Genre?> GetGenreById(int id)
        {
            return await context.Genres.FindAsync(id);
        }
    }
}
