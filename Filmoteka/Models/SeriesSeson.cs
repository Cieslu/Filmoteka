#nullable disable

namespace Filmoteka.Models
{
    public class SeriesSeson
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public int SesonId { get; set; }


        public Series Series { get; set; }
        public Seson Seson { get; set; }
    }
}
