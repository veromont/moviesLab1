using moviesAPI.Models.db;

namespace moviesAPI.Models
{
    public class PdfTicketModel: Ticket
    {
        public PdfTicketModel(Ticket ticket, string movieTitle, string hallName) 
        {
            Id = ticket.Id;
            SessionId = ticket.SessionId;
            SeatNumber = ticket.SeatNumber;
            MovieTitle = movieTitle;
            ClientId = ticket.ClientId;
            StartTime = ticket.Session == null ? "" : ticket.Session.StartTime.ToString();
            EndTime = ticket.Session == null ? "" : ticket.Session.EndTime.ToString();
            Price = ticket.Session == null ? -1 : ticket.Session.Price;
            HallName = hallName;
        }
        public string MovieTitle { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string HallName { get; set; }
        public double Price { get; set; }
        public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
            { nameof(Id), "ID-unicode" },
            { nameof(SessionId), "Номер сеансу" },
            { nameof(SeatNumber), "Місце" },
            { nameof(MovieTitle), "Назва фільму" },
            { nameof(Client), "Власник" },
            { nameof(StartTime), "Час початку сеансу" },
            { nameof(EndTime), "Час закінчення сеансу" },
            { nameof(Price), "Ціна квитка" },
            { nameof(HallName), "Зал" },
    };
    }
}
