using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace moviesAPI.Models.db
{
    public partial class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }
    }
}
