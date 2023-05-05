using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace moviesAPI.Models;

public partial class Ticket
{
    public string Id { get; set; } = null!;
    public string SessionId { get; set; } = null!;
    public string SeatNumber { get; set; } = null!;
    public decimal Price { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Session? Session { get; set; } = null!;
}
