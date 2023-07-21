using moviesAPI.Models.EntityModels;

namespace moviesAPI.Models
{
	public class PdfTicketModel : Ticket
	{
		public PdfTicketModel(Ticket ticket, string movieTitle, string hallName)
		{
			Id = ticket.Id;
			SessionId = ticket.SessionId;
			SeatNumber = ticket.SeatNumber;
			MovieTitle = movieTitle;
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

	}
}
