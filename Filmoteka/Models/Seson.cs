#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Filmoteka.Models
{
    public class Seson
    {
        [Key]
        public int Id { get; set; }
        public int SesonCounter { get; set; }
        public string Name { get; set; }

        public ICollection<SeriesSeson> SeriesSeson { get; set; }
        public ICollection<Episode> Epiosodes{ get; set; }
    }
}
