#nullable disable
using Filmoteka.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmoteka.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MovieSeriesGenre> MovieSeriesGenres { get; set; }
        public DbSet<Seson> Sesons { get; set; }
        public DbSet<SeriesSeson> SeriesSesons { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new Genre[]
            {
                new Genre {Name = "Akcja", Id = 1},
                new Genre {Name = "Komedia", Id = 2},
                new Genre {Name = "Dramat", Id = 3},
                new Genre {Name = "Horror", Id = 4},
                new Genre {Name = "Romans", Id = 5},
                new Genre {Name = "Przygodowy", Id = 6},
                new Genre {Name = "Thriller", Id = 7},
                new Genre {Name = "Sci-Fi", Id = 8},
                new Genre {Name = "Kryminał", Id = 9},
                new Genre {Name = "Fantasy", Id = 10},
                new Genre {Name = "Dokumentalny", Id = 11},
                new Genre {Name = "Familijny", Id = 12},
                new Genre {Name = "Animacja", Id = 13},
                new Genre {Name = "Wojenny", Id = 14},
                new Genre {Name = "Musical", Id = 15},
                new Genre {Name = "Historyczny", Id = 16},
                new Genre {Name = "Przyrodniczy", Id = 17}
            });

            modelBuilder.Entity<User>()
                .Property(e => e.NickName)
                .HasMaxLength(250);
            
        }
    }
}
