using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace moviesAPI.Models.db
{
    public partial class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }
    }
}
