using NuGet.Configuration;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.db;

public partial class Ticket
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public int SeatNumber { get; set; }
    public Guid ClientId { get; set; }

    [JsonIgnore]
    public virtual Session? Session { get; set; }
}
