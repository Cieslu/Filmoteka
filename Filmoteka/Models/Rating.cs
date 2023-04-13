#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int? EpisodeId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }

        public Movie Movie { get; set; }
        public Series Series { get; set; }
        public Episode Episode { get; set; }
    }
}
