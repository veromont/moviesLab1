using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.db;

public partial class Session
{
    public string Id { get; set; } = null!;
    public string MovieId { get; set; } = null!;
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    [JsonIgnore]
    public virtual Hall? Hall { get; set; } = null!;

    [JsonIgnore]
    public virtual Movie? Movie { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Ticket>? SessionTickets { get; set; } = new List<Ticket>();
}
