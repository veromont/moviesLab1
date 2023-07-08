using moviesAPI.Models.db;

namespace moviesAPI.Models
{
    public class ServedSession: Session
    {
        public ServedSession(Session s, string movieName, string hallName) 
        {
            HallId = s.HallId;
            MovieId = s.MovieId;
            Hall = hallName;
            Movie = movieName;
            Id = s.Id;
            StartTime = s.StartTime;
            EndTime = s.EndTime;
        }
        public string Movie { get; set; }
        public string Hall { get; set; }
    }
}
