using Microsoft.AspNetCore.Identity;

namespace moviesAPI.Models.EntityModels;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public byte[]? Image { get; set; }
    public string? FavouriteMovieId { get; set; }
    public string? Bio { get; set; }
}

