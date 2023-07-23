using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using moviesAPI.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace moviesAPI.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам`ятати?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
