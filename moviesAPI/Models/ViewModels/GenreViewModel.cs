using moviesAPI.Models.EntityModels;
using System.ComponentModel.DataAnnotations;

namespace moviesAPI.Models.ViewModels
{
    public class GenreViewModel : Genre
    {
        [Display(Name = "Кількість фільмів цього жанру")]
        public int MovieWithThisGenreCount { get; set; }

        public GenreViewModel(Genre genre)
        {
            MovieWithThisGenreCount = genre.Movies?.Count ?? 0;
            Id = genre.Id;
            Name = genre.Name;
        }
    }
}
