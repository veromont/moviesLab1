using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels
{
    public partial class Genre : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }

		[JsonIgnore]
		public static Dictionary<string, string> TranslationMap { get; } = new Dictionary<string, string>{
			{ nameof(Id), "ID-unicode" },
			{ nameof(Name), "Назва" },
		};
	}
}
