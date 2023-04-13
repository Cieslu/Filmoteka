#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nazwa hostingu")]
        [StringLength(30)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Name { get; set; }
        [Display(Name = "Jakość")]
        [StringLength(30)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Quality { get; set; }
        [Display(Name = "Link URL")]
        [StringLength(230)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [RegularExpression("https://(vidoza.net|upstream.to)/embed-([A-ząćęłńóśźżĄĘŁŃÓŚŹŻ0-9]{12}).html$", ErrorMessage = "Niepoprawny link.")]
        public string URL { get; set; }
        public string Username { get; set; }
        public int? MovieId { get; set; }
        public int? EpisodeId { get; set; }

        public Movie Movie { get; set; }
        public Episode Episode { get; set; }
    }
}
