using moviesAPI.Models.db;
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

        public string isGenreInvalid(Genre Genre)
        {
            return string.Empty;
        }
        public string isHallInvalid(Hall hall)
        {
            const int MAX_CAPACITY = 10000;
            const int MIN_CAPACITY = 0;

            List<string> errorMessages = new List<string>();

            if (hall.Capacity <= MIN_CAPACITY || hall.Capacity >= MAX_CAPACITY) 
                errorMessages.Add($"некоректна місткість залу, коректна більша за {MIN_CAPACITY} і менша за {MAX_CAPACITY}");

            return formatResult(errorMessages);
        }
        public async Task<string> isMovieInvalid(Movie movie)
        {
            const int TOO_LONG = 8;
            const int TOO_SHORT = 1;
            const int START_OF_CINEMATOGRAPHY = 1890;
            const int MIN_RATING = 0;
            const int MAX_RATING = 10;

            var Genres = await repository.Get<Genre>();
            var movieGenre = Genres.Where(genre => genre.Id == movie.GenreId).First();

            List<string> errorMessages = new List<string>();

            if (movie.Duration.Hour > TOO_LONG || movie.Duration.Hour < TOO_SHORT)
                errorMessages.Add("тривалість фільму суперечить здоровому ґлузду");

            if (movie.ReleaseDate.Year <= START_OF_CINEMATOGRAPHY)
                errorMessages.Add("дата виходу суперечить здоровому ґлузду");

            if (movie.Rating < MIN_RATING || movie.Rating > MAX_RATING)
                errorMessages.Add($"рейтинг має бути від {MIN_RATING} до {MAX_RATING}");

            if (movieGenre == null && movie.GenreId != null)
                errorMessages.Add("такого жанру не знайдено");

            if (movie.GenreId != null)
                movie.Genre = movieGenre;

            return formatResult(errorMessages);
        }
        public async Task<string> isSessionInvalid(Session session)
        {
            const int MIN_PRICE = 0;

            var Movies = await repository.Get<Movie>();
            var Halls = await repository.Get<Hall>();
            var Sessions = await repository.Get<Session>();

            var movie = Movies.Where(s => s.Id == session.MovieId).First();
            var hall = Halls.Where(h => h.Id == session.HallId).First();

            List<string> errorMessages = new List<string>();

            if (movie == null) 
                errorMessages.Add("некоректний фільм");

            if (hall == null)
                errorMessages.Add("некоректний зал");

            if (!hall.IsAvailable) 
                errorMessages.Add($"зал {hall.Name} тимчасово недоступний");

            if (session.EndTime <= session.StartTime) 
                errorMessages.Add("час сеансу не може бути від'ємним");

            var sessionDuration = TimeOnly.FromTimeSpan(session.EndTime - session.StartTime);
            if (sessionDuration < movie.Duration) 
                errorMessages.Add($"сеанс має бути довшим за фільм, фільм триває {movie.Duration.ToShortTimeString}");

            var overlayingSessions = from s in Sessions
                                     where ((s.StartTime > session.StartTime && s.StartTime < session.EndTime)
                                           || (s.EndTime > session.StartTime && s.EndTime < session.EndTime)
                                           || (s.StartTime < session.StartTime && s.EndTime > session.EndTime))
                                           && s.HallId == session.HallId
                                     select s;
            if (overlayingSessions.Count() > 0)
                errorMessages.Add("заплановано інші сессії на цей час");

            if (movie.ReleaseDate > DateOnly.FromDateTime(session.StartTime)) 
                errorMessages.Add($"фільм ще не вийде станом на {session.StartTime.ToLocalTime}");

            if (session.Price < MIN_PRICE)
                errorMessages.Add($"ціна менша за мінімальну {MIN_PRICE}");

            return formatResult(errorMessages);
        }
        public async Task<string> isTicketInvalid(Ticket ticket)
        {
            var Sessions = await repository.Get<Session>();
            var Halls = await repository.Get<Hall>();

            var ticketsSession = Sessions.Where(s => s.Id == ticket.SessionId).First();
            var ticketsHall = Halls.Where(h => h.Id == ticketsSession.HallId).First();

            List<string> errorMessages = new List<string>();

            if (ticketsSession == null) 
                errorMessages.Add($"сеансу з id {ticket.SessionId} не існує");

            if (ticketsSession.SessionTickets != null)
            {
                var sameSeatTickets = ticketsSession.SessionTickets.Where(t => t.SeatNumber == ticket.SeatNumber);
                if (sameSeatTickets.Count() > 0) 
                    errorMessages.Add($"місце {ticket.SeatNumber} уже заброньовано");
            }

            if (ticketsHall.Capacity < ticket.SeatNumber || ticket.SeatNumber <= 0) 
                errorMessages.Add($"зал {ticketsHall.Name} не містить місця під номером {ticket.SeatNumber}");

            if (!ticketsHall.IsAvailable) 
                errorMessages.Add($"зал {ticketsHall.Name} не доступний");

            return formatResult(errorMessages);
        }
        public async Task<string> isClientInvalid(Client client)
        {
            return "";
        }
        private string formatResult(IEnumerable<string> errorMessages)
        {
            return string.Join("\n", errorMessages);
        }
    }
}
