#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<MovieSeriesGenre> MovieSeriesGenres { get; set; }
    }
}
