#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Filmoteka.Models.ViewModels
{
    public class MovieGenreCommentsViewModel
    {
        public Movie Movie { get; set; }
        public Series Series { get; set; }
        public Link Link { get; set; }
        public Comment Comment { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Seson> Sesons { get; set; } = new List<Seson>();
        public ICollection<Link> Links { get; set; } = new List<Link>();

        public ICollection<Movie> MoviesML { get; set; } = new List<Movie>();
        public ICollection<Series> SeriesML { get; set; } = new List<Series>();

        [NotMapped]
        public int Rate { get; set; }
        [NotMapped]
        public float Avg { get; set; }
    }
}
