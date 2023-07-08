namespace moviesAPI.Models.db
{
    public partial class Client
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
