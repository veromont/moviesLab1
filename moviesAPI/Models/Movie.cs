using System;
using System.Collections.Generic;

namespace moviesAPI.Models;

public partial class Movie
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Director { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public string Genre { get; set; } = null!;

    public double Rating { get; set; }

    public TimeOnly Duration { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
