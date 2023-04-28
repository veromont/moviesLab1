using System;
using System.Collections.Generic;

namespace moviesAPI.Models;

public partial class Ticket
{
    public string Id { get; set; } = null!;

    public string SessionId { get; set; } = null!;

    public string MovieId { get; set; } = null!;

    public int HallId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Hall Hall { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
}
