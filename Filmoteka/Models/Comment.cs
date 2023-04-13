#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa użytkownika")]
        [StringLength(30)]
        [Required(ErrorMessage = "Pole {0} jest wymagana.")]
        public string Name { get; set; }

        [Display(Name = "Komentarz")]
        [StringLength(200)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Description { get; set; }

        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int? EpisodeId { get; set; }
        public DateTime DateTime { get; set; }

        public Movie Movie { get; set; }
        public Series Series { get; set; }
        public Episode Episode { get; set; }
    }
}
