namespace moviesAPI.Models
{
    public partial class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Movie>? Movies { get; set; }
    }
}
