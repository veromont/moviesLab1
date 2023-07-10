using moviesAPI.Models.db;

namespace moviesAPI.Models
{
    public class PdfTicketModel: Ticket
    {
        public PdfTicketModel(Ticket t, string title, string startTime, string endTime, string hall) 
        {
            Id = t.Id;
            SessionId = t.SessionId;
            SeatNumber = t.SeatNumber;
            MovieTitle = title;
            ClientId = t.ClientId;
            StartTime = startTime;
            EndTime = endTime;
            HallName = hall;
        }
        public string MovieTitle { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string HallName { get; set; }
        public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
            { nameof(Id), "ID-unicode" },
            { nameof(SessionId), "Номер сеансу" },
            { nameof(SeatNumber), "Місце" },
            { nameof(MovieTitle), "Назва фільму" },
            { nameof(Client), "Власник" },
            { nameof(StartTime), "Час початку сеансу" },
            { nameof(EndTime), "Час закінчення сеансу" },
            { nameof(HallName), "Зал" },
    };
    }
}
