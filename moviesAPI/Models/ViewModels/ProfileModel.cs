using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace moviesAPI.Models.ViewModels
{
    public class ProfileInputModel
    {
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Phone]
        [Display(Name = "Номер телефону")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Фото")]
        public byte[]? Image { get; set; }

        [Display(Name = "Улюблений фільм")]
        public string? FavouriteMovieId { get; set; }

        [Display(Name = "Користувач")]
        public string? Header { get; set; }

        [Display(Name = "Про себе")]
        public string? Bio { get; set; }
    }

    public class ProfileModel
    {
        [Display(Name = "Логін")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public ProfileModel()
        {
            Input = new ProfileInputModel();
        }
        public ProfileModel(User user)
        {
            Input = new ProfileInputModel();
            Username = user.UserName;
            Input.PhoneNumber = user.PhoneNumber;
            Input.Name = user.Name;
            Input.Surname = user.Surname;
            Input.FavouriteMovieId = user.FavouriteMovieId;
            Input.Image = user.Image;
            Input.Bio = user.Bio;
        }
        public void UpdateUser(User user)
        {
            user.Name = Input.Name ?? user.Name;
            user.Surname = Input.Surname ?? user.Surname;
            user.PhoneNumber = Input.PhoneNumber ?? user.PhoneNumber;
            user.FavouriteMovieId = Input.FavouriteMovieId ?? user.FavouriteMovieId;
            user.Image = Input.Image ?? user.Image;
            user.Bio = Input.Bio ?? user.Bio;
        }
    }
}
