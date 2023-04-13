#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace Filmoteka.Models.ViewModels
{
    public class EpisodeVievModel
    {
        public Episode Episode { get; set; }

        public Link Link { get; set; }

        public Comment Comment { get; set; }

        public MyLibrary ML { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Link> Links { get; set; } = new List<Link>();

        public ICollection<Movie> MoviesML { get; set; } = new List<Movie>();
        public ICollection<Series> SeriesML { get; set; } = new List<Series>();
        public ICollection<Episode> EpisodeList { get; set; }

        [NotMapped]
        public int Rate { get; set; }
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
    }
}
