using moviesAPI.Models.EntityModels;

namespace moviesAPI.Models.ViewModels
{
    public class MovieViewModel : Movie
    {
        public string ReleaseDateString { get; set; }
        public string DurationString { get; set; }
        public string GenreString { get; set; }
        public double AverageTicketCost { get; set; }
        public int TicketsSold { get; set; }

        public MovieViewModel(Movie movie) 
        {
            Id = movie.Id;
            Title = movie.Title;
            Director = movie.Director;
            ReleaseDate = movie.ReleaseDate;
            Duration = movie.Duration;
            Plot = movie.Plot;
            GenreId = movie.GenreId;
            Rating = movie.Rating;
        }
    }
}
