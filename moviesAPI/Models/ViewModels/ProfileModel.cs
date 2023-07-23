using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace moviesAPI.Models.ViewModels
{
    public class ProfileInputModel
    {
        [Display(Name ="Ім'я")]
        public string Name { get; set; }

        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Phone]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }
    }
    public class ProfileModel
    {
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }
        public ProfileModel()
        {
            Input = new ProfileInputModel();
        }
    }
}
