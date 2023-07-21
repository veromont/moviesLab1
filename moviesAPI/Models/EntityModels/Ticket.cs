using NuGet.Configuration;
using System.IO;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Ticket
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public int SeatNumber { get; set; }
    public Guid USerId { get; set; }

    [JsonIgnore]
    public virtual Session? Session { get; set; }

	[JsonIgnore]
	public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
		{ nameof(Id), "ID-unicode" },
		{ nameof(SessionId), "id сесії" },
		{ nameof(SeatNumber), "Номер місця" },
		{ nameof(USerId), "id клієнта" },
	};
}
