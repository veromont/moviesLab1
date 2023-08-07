using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.EntityModels
{
    public partial class Genre
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name="Назва")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserGenreConnection>? Clients { get; set; }
    }
}
