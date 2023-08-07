using Newtonsoft.Json;

namespace moviesAPI.Models.EntityModels
{
    public class UserGenreConnection
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public int GenreId { get; set; }

        [JsonIgnore]
        public virtual Genre Genre { get; set; }
    }
}
