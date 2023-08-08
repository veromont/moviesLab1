using moviesAPI.CustomAttributes;
using NuGet.Configuration;
using System.IO;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Ticket
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public int SeatNumber { get; set; }
    public Guid UserId { get; set; }

    [JsonIgnore]
	[TableIgnore]
    public virtual Session? Session { get; set; }
}
