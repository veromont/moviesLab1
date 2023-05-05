using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace moviesAPI.Models;

public partial class Movie
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Director { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public double Rating { get; set; }
    public TimeOnly Duration { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();
    public int GenreId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public Genre? Genre { get; set; }
}
