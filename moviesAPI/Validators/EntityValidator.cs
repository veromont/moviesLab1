using moviesAPI.Models;
using moviesAPI.Models.EntityModels;
using moviesAPI.Repositories;

namespace moviesAPI.Validators
{
    /// <summary>
    /// This classes methods return empty string if object is valid, and error messages as string otherwise
    /// </summary>
    public partial class EntityValidator
    {
        private readonly GenericCinemaRepository repository;
        public EntityValidator(GenericCinemaRepository repository)
        {
            this.repository = repository;
        }

        public Dictionary<string,string> isGenreInvalid(Genre Genre)
        {
            return new Dictionary<string, string>();
        }
        public Dictionary<string,string> isHallInvalid(Hall hall)
        {
            const int MAX_CAPACITY = 10000;
            const int MIN_CAPACITY = 0;

            var errorMessages = new Dictionary<string, string>();

            if (hall.Capacity <= MIN_CAPACITY || hall.Capacity >= MAX_CAPACITY) 
                errorMessages.Add(nameof(hall.Capacity),$"некоректна місткість залу, коректна більша за {MIN_CAPACITY} і менша за {MAX_CAPACITY}");

            return errorMessages;
        }
        public async Task<Dictionary<string, string>> isMovieInvalid(Movie movie)
        {
            const int TOO_LONG = 8;
            const int TOO_SHORT = 1;
            const int START_OF_CINEMATOGRAPHY = 1890;
            const int MIN_RATING = 0;
            const int MAX_RATING = 10;

            var Genres = await repository.GetAll<Genre>();
            var movieGenre = Genres.Where(genre => genre.Id == movie.GenreId).First();

            var errorMessages = new Dictionary<string,string>();

            if (movie.Duration.Hour > TOO_LONG || movie.Duration.Hour < TOO_SHORT)
                errorMessages.Add(nameof(Movie.Duration),"тривалість фільму суперечить здоровому ґлузду");

            if (movie.ReleaseDate.Year <= START_OF_CINEMATOGRAPHY)
                errorMessages.Add(nameof(Movie.ReleaseDate), "дата виходу суперечить здоровому ґлузду");

            if (movie.Rating < MIN_RATING || movie.Rating > MAX_RATING)
                errorMessages.Add(nameof(movie.Rating),$"рейтинг має бути від {MIN_RATING} до {MAX_RATING}");

            if (movieGenre == null && movie.GenreId != null)
                errorMessages.Add(nameof(Movie.GenreId), "такого жанру не знайдено");

            if (movie.GenreId != null)
                movie.Genre = movieGenre;

            return errorMessages;
        }
        public async Task<Dictionary<string, string>> isSessionInvalid(Session session)
        {
            const int MIN_PRICE = 0;

            var Movies = await repository.GetAll<Movie>();
            var Halls = await repository.GetAll<Hall>();
            var Sessions = await repository.GetAll<Session>();

            var movie = Movies.Where(s => s.Id == session.MovieId).First();
            var hall = Halls.Where(h => h.Id == session.HallId).First();

            var errorMessages = new Dictionary<string, string>();

            if (movie == null) 
                errorMessages.Add(nameof(session.MovieId),"некоректний фільм");

            if (hall == null)
                errorMessages.Add(nameof(session.HallId), "некоректний зал");

            if (!hall.IsAvailable) 
                errorMessages.Add(nameof(session.HallId), $"зал {hall.Name} тимчасово недоступний");

            if (session.EndTime <= session.StartTime) 
                errorMessages.Add(nameof(session.EndTime), "час сеансу не може бути від'ємним");

            var sessionDuration = TimeOnly.FromTimeSpan(session.EndTime - session.StartTime);
            if (sessionDuration < movie.Duration) 
                errorMessages.Add(nameof(session.EndTime), $"сеанс має бути довшим за фільм, фільм триває {movie.Duration.ToShortTimeString}");

            var overlayingSessions = from s in Sessions
                                     where ((s.StartTime > session.StartTime && s.StartTime < session.EndTime)
                                           || (s.EndTime > session.StartTime && s.EndTime < session.EndTime)
                                           || (s.StartTime < session.StartTime && s.EndTime > session.EndTime))
                                           && s.HallId == session.HallId
                                     select s;
            if (overlayingSessions.Count() > 0)
                errorMessages.Add(nameof(session.StartTime), "заплановано інші сеанси на цей час");

            if (movie.ReleaseDate > DateOnly.FromDateTime(session.StartTime)) 
                errorMessages.Add(nameof(session.StartTime), $"фільм ще не вийде станом на {session.StartTime.ToLocalTime}");

            if (session.Price < MIN_PRICE)
                errorMessages.Add(nameof(session.Price), $"ціна менша за мінімальну {MIN_PRICE}");

            return errorMessages;
        }
        public async Task<Dictionary<string, string>> isTicketInvalid(Ticket ticket)
        {
            var Sessions = await repository.GetAll<Session>();
            var Halls = await repository.GetAll<Hall>();

            var ticketsSession = Sessions.Where(s => s.Id == ticket.SessionId).First();
            var ticketsHall = Halls.Where(h => h.Id == ticketsSession.HallId).First();

            var errorMessages = new Dictionary<string,string>();

            if (ticketsSession == null) 
                errorMessages.Add(nameof(Ticket.SessionId), $"сеансу з id {ticket.SessionId} не існує");

            if (ticketsSession.SessionTickets != null)
            {
                var sameSeatTickets = ticketsSession.SessionTickets.Where(t => t.SeatNumber == ticket.SeatNumber);
                if (sameSeatTickets.Count() > 0) 
                    errorMessages.Add(nameof(Ticket.SeatNumber), $"місце {ticket.SeatNumber} уже заброньовано");
            }

            if (ticketsHall.Capacity < ticket.SeatNumber || ticket.SeatNumber <= 0) 
                errorMessages.Add(nameof(Ticket.SeatNumber), $"зал {ticketsHall.Name} не містить місця під номером {ticket.SeatNumber}");

            if (!ticketsHall.IsAvailable) 
                errorMessages.Add(nameof(Ticket.SessionId), $"зал {ticketsHall.Name} тимчасово не доступний");

            return errorMessages;
        }
    }
}
