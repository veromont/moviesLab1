using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Movie : Entity
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

	[JsonIgnore]
	public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
		{ nameof(Id), "ID-unicode" },
		{ nameof(Title), "Місткість" },
		{ nameof(Director), "Ім'я" },
		{ nameof(ReleaseDate), "Дата виходу" },
		{ nameof(Rating), "Оцінка" },
		{ nameof(Duration), "Тривалість" },
		{ nameof(GenreId), "Жанр id" },
	};
}
