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
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [DisplayName("Рейтинг")]
    public double Rating { get; set; }

    [DisplayName("Тривалість")]
    [DataType(DataType.Time)]
    public TimeSpan Duration { get; set; }

    [DisplayName("Жанр")]
    public int? GenreId { get; set; }

    [TableIgnore]
    [DisplayName("Сюжет")]
    public string? Plot { get; set; }


    [JsonIgnore]
    [TableIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

}
