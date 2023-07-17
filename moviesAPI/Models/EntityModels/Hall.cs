using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels;

public partial class Hall : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }

    [JsonIgnore]
    public virtual ICollection<Session>? Sessions { get; set; } = new List<Session>();

	[JsonIgnore]
	public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
		{ nameof(Id), "ID-unicode" },
			{ nameof(Capacity), "Місткість" },
			{ nameof(Name), "Ім'я" },
			{ nameof(IsAvailable), "Доступність" },
		};
}
