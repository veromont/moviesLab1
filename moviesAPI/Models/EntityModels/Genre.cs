using moviesAPI.CustomAttributes;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace moviesAPI.Models.EntityModels
{
    public partial class Genre
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Назва")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        [TableIgnore]
        public virtual ICollection<Movie>? Movies { get; set; }

        [JsonIgnore]
        [TableIgnore]
        public virtual ICollection<UserGenreConnection>? Clients { get; set; }
    }
}
