#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class MovieSeriesGenre
    {
        [Key]
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int? GenreId { get; set; }

        public Movie Movie { get; set; }
        public Series Series { get; set; }
        public Genre Genres { get; set; }
    }
}
