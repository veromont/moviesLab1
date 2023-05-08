using NuGet.Configuration;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.db;

public partial class Ticket
{
    public string Id { get; set; } = null!;
    public string SessionId { get; set; } = null!;
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }

    [JsonIgnore]
    public virtual Session? Session { get; set; } = null!;
}
