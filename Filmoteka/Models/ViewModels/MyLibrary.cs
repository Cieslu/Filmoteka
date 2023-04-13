#nullable disable
namespace Filmoteka.Models.ViewModels
{
    public class MyLibrary
    {
        public int GenreId { get; set; }
        public ICollection<Movie> MovieList { get; set; } = new List<Movie>();
        public ICollection<Series> SeriesList { get; set; } = new List<Series>();
        public ICollection<Movie> MoviesML { get; set; } = new List<Movie>();
        public ICollection<Series> SeriesML { get; set; } = new List<Series>();
        public string movieSearch { get; set; }
        public ICollection<User> NickMovie { get; set; } = new List<User>();
        public ICollection<User> NickSeries { get; set; } = new List<User>();
    }
}
