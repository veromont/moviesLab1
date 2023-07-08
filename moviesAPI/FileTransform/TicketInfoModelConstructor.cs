using moviesAPI.Models;
using moviesAPI.Models.CinemaContext;
using moviesAPI.Models.db;

namespace moviesAPI.FileTransform
{
    public class TicketInfoModelConstructor
    {
        CinemaContext _context;
        public TicketInfoModelConstructor(CinemaContext context)
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
