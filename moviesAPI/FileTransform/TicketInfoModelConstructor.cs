using moviesAPI.Models;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;

namespace moviesAPI.FileTransform
{
    public class TicketInfoModelConstructor
    {
        MovieCinemaLabContext _context;
        public TicketInfoModelConstructor(MovieCinemaLabContext context)
        {
            _context = context;
        }
        public PdfTicketModel? getpdfTicketModel(Ticket ticket)
        {
            var session = _context.Sessions.Find(ticket.SessionId);
            if (session == null) return null;
            var movie = _context.Movies.Find(session.MovieId);
            var hall = _context.Halls.Find(session.HallId);
            if (movie == null) return null;
            if (hall == null) return null;

            var result = new PdfTicketModel(ticket, movie.Title, session.StartTime.ToString(), hall.Name);
            return result;
        }
    }
}
