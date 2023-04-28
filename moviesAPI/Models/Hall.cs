using System;
using System.Collections.Generic;

namespace moviesAPI.Models;

public partial class Hall
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
