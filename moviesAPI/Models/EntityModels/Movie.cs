using moviesAPI.CustomAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Movie
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Назва")]
    public string Title { get; set; } = null!;

    [DisplayName("Режисер")]
    public string Director { get; set; } = null!;

    [DisplayName("Дата виходу")]
    public DateOnly ReleaseDate { get; set; }

    [DisplayName("Рейтинг")]
    public double Rating { get; set; }

    [DisplayName("Тривалість")]
    public TimeOnly Duration { get; set; }

    [DisplayName("Id жанру")]
    public int? GenreId { get; set; }

    [TableIgnore]
    public string? Plot { get; set; }

    [JsonIgnore]
    [TableIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

    [JsonIgnore]
    [TableIgnore]
    public Genre? Genre { get; set; }

}
