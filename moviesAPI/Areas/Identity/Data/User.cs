using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using moviesAPI.Models.EntityModels;
using Newtonsoft.Json;

namespace moviesAPI.Areas.Identity.Data;

public class User : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public byte[]? Image { get; set; }
    public string? FavouriteMovieId { get; set; }
    public string? Bio { get; set; }
}

