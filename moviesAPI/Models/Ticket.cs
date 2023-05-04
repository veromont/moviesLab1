using System;
using System.Collections.Generic;

namespace moviesAPI.Models;

public partial class Ticket
{
    public string Id { get; set; } = null!;
    public string SessionId { get; set; } = null!;
    public string SeatNumber { get; set; } = null!;
    public decimal Price { get; set; }
    public virtual Session Session { get; set; } = null!;
}
