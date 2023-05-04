using System;
using System.Collections.Generic;

namespace moviesAPI.Models;

public partial class Session
{
    public string Id { get; set; } = null!;
    public string MovieId { get; set; } = null!;
    public int HallId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public virtual Hall Hall { get; set; } = null!;
    public virtual Movie Movie { get; set; } = null!;
    public virtual ICollection<Ticket>? Tickets { get; set; } = new List<Ticket>();
}
