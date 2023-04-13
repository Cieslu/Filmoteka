#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Filmoteka.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tytuł")]

        public string Title { get; set; }
        [Display(Name = "Opis odcinka")]
        public string Description { get; set; }
        public string Photo { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data wydania")]
        public DateTime Date { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Display(Name = "Dodaj zdjęcie")]
        [NotMapped]
        public IFormFile Image { get; set; }
        public int SesonId { get; set; }

        [NotMapped]
        public int Rate { get; set; }
        [NotMapped]
        public float Avg { get; set; }
        [NotMapped]
        public int AvgCount { get; set; }
        [NotMapped]
        public float TMDBRate { get; set; }
        [NotMapped]
        public int TMDBRateCount { get; set; }
        [NotMapped]
        public string TMDBlink { get; set; }
        [NotMapped]
        public int CommentCount { get; set; }

        public Seson Seson { get; set; }
    }
}
