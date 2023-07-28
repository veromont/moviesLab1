using Newtonsoft.Json;

namespace moviesAPI.Models.EntityModels
{
    public class ClientLikesGenre
    {
        public string Username { get; set; }
        public int GenreId { get; set; }

        [JsonIgnore]
        public virtual Genre Genre { get; set; }
    }
}
