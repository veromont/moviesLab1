using moviesAPI.Models.db;

namespace moviesAPI.Models
{
    public class PdfTicketModel: Ticket
    {
        public PdfTicketModel(Ticket t, string title,string time, string hall) 
        {
            Id = t.Id;
            SessionId = t.SessionId;
            Price = t.Price;
            SeatNumber = t.SeatNumber;
            MovieTitle = title;
            Time = time;
            HallName = hall;
        }
        public string MovieTitle { get; set; }
        public string Time { get; set; }
        public string HallName { get; set; }
        public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
            { nameof(Id), "ID-unicode" },
            { nameof(SessionId), "Номер сеансу" },
            { nameof(SeatNumber), "Місце" },
            { nameof(Price), "Ціна квитка(грн)" },
            { nameof(MovieTitle), "Назва фільму" },
            { nameof(Time), "Час початку сеансу" },
            { nameof(HallName), "Зал" },
    };
    }
}
