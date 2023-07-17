using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Session : Entity
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double Price { get; set; }

    [JsonIgnore]
    public virtual Hall? Hall { get; set; }

    [JsonIgnore]
    public virtual Movie? Movie { get; set; }

    [JsonIgnore]
    public virtual ICollection<Ticket>? SessionTickets { get; set; } = new List<Ticket>();

	[JsonIgnore]
	public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
		{ nameof(Id), "ID-unicode" },
		{ nameof(MovieId), "Фільм id" },
		{ nameof(HallId), "Зал id" },
		{ nameof(StartTime), "Початок" },
		{ nameof(EndTime), "Кінець" },
		{ nameof(Price), "Ціна" },
	};
}
