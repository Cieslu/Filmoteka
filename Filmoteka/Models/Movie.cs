#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filmoteka.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tytuł")]
        [StringLength(100)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Title { get; set; }

        [Display(Name = "Reżyseria")]
        [StringLength(100)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Director { get; set; }

        [Display(Name = "Rok produkcji")]
        [StringLength(4)]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Wymagane 4 liczby np. 1234")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string RelaseDate { get; set; }

        [Display(Name = "Opis filmu")]
        [StringLength(700)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Description { get; set; }

        [Display(Name = "Link do zdjęcia")]
        public string Photo { get; set; }

        [Display(Name = "Link do zwiastunu")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Video { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "{0} jest wymagana.")]
        public List<int> MovieSeriesGenreId { get; set; } = new List<int>();

        [NotMapped]
        [Display(Name = "Dodaj zdjęcie")]
        public IFormFile Image { get; set; }

        public string UserId { get; set; }

        public bool IsConfirm { get; set; } = false;

        [NotMapped]
        public string NickName { get; set; }

        [NotMapped]
        public float Avg { get; set; }
        [NotMapped]
        public float AvgCount { get; set; }
        [NotMapped]
        public float TMDBRate { get; set; }
        [NotMapped]
        public int TMDBRateCount { get; set; }
        [NotMapped]
        public string TMDBlink { get; set; }

        public ICollection<MovieSeriesGenre> MovieSeriesGenres { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
