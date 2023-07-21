using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Session
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

}
