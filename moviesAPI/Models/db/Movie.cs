using System.Text.Json.Serialization;

namespace moviesAPI.Models.db;

public partial class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Director { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public double Rating { get; set; }
    public TimeOnly Duration { get; set; }
    public int? GenreId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

    [JsonIgnore]
    public Genre? Genre { get; set; }
}
