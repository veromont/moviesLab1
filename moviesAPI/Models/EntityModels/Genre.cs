using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels
{
    public partial class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }

        [JsonIgnore]
        public virtual ICollection<ClientLikesGenre>? Clients { get; set; }

    }
}
