#nullable disable
using Filmoteka.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Filmoteka.Data;
using Filmoteka.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Filmoteka.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Filmoteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private MyLibrary library = new MyLibrary();

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<User> userManager)
        {
            _logger = logger;
            _context = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            var rates = _context.Ratings.ToList();
            var movies = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var series = _context.Series.Where(x => x.IsConfirm == true).ToList();

            movies.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.MovieId == x.Id && x.IsConfirm == true).Select(s => s.Rate).ToList();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    x.Avg = (float)avg;
                    library.MovieList.Add(x);
                }
            });

            library.MovieList = library.MovieList.OrderByDescending(x => x.Avg).Take(5).ToList();

            series.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.SeriesId == x.Id).Select(s => s.Rate).ToList();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    x.Avg = (float)avg;
                    library.SeriesList.Add(x);
                }
            });

            library.SeriesList = library.SeriesList.OrderByDescending(x => x.Avg).Take(5).ToList();

            return View(library);
        }

        public IActionResult Films()
        {
            TempData["xd"] = null;
            var rates = _context.Ratings.ToList();
            var movies = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var UserId = _userManager.GetUserId(User);

            movies.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.MovieId == x.Id).Select(s => s.Rate).ToList();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    x.Avg = (float)avg;
                    x.AvgCount = tmpRates.Count;
                }
            });

            library.MovieList = movies;
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            ViewBag.GenreId = _context.Genres.ToList();
            //ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");


            return View(library);
        }

        public IActionResult Series()
        {
            TempData["xd"] = null;
            var rates = _context.Ratings.ToList();
            var series = _context.Series.Where(x => x.IsConfirm == true).ToList();
            var UserId = _userManager.GetUserId(User);

            series.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.SeriesId == x.Id).Select(s => s.Rate).ToList();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    x.Avg = (float)avg;
                    x.AvgCount = tmpRates.Count;
                }
            });

            library.SeriesList = series;
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            ViewBag.GenreId = _context?.Genres.ToList();

            return View(library);
        }

        public IActionResult GenreSearchMovie(int id)
        {
            var genre = _context.MovieSeriesGenres.Where(x => x.GenreId == id && x.MovieId != null && x.Movie.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            ViewBag.genre = "("+_context.Genres.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault()+")";
            ViewBag.genre2 = _context.Genres.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            genre.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.MovieId == x.MovieId).Select(s => s.Rate).ToList();
                var movie = _context.Movies.Where(y => y.Id == x.MovieId && y.IsConfirm == true).FirstOrDefault();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    movie.Avg = (float)avg;
                    movie.AvgCount = tmpRates.Count;
                }

                library.MovieList.Add(movie);
            });

            ViewBag.check = true;
            if(library.MovieList.Count == 0)
            {
                ViewBag.check = false;
            }

            TempData["xd"] = id;
            
            ViewBag.GenreId = _context.Genres.ToList();
            return View("Films", library);
        }

        public IActionResult GenreSearchSeries(int id)
        {
            var genre = _context.MovieSeriesGenres.Where(x => x.GenreId == id && x.SeriesId != null && x.Series.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            ViewBag.genre = "(" + _context.Genres.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault() + ")";
            ViewBag.genre2 = _context.Genres.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();

            genre.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.SeriesId == x.SeriesId).Select(s => s.Rate).ToList();
                var series = _context.Series.Where(y => y.Id == x.SeriesId && y.IsConfirm == true).FirstOrDefault();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    series.Avg = (float)avg;
                    series.AvgCount = tmpRates.Count;
                }

                library.SeriesList.Add(series);
            });

            ViewBag.check = true;
            if (library.SeriesList.Count == 0)
            {
                ViewBag.check = false;
            }

            TempData["xd"] = id;

            ViewBag.GenreId = _context.Genres.ToList();
            return View("Series", library);
        }

        public IActionResult SearchMovie(MyLibrary ml)
        {
            var movies = _context.Movies.Where(x => x.Title.Contains(ml.movieSearch) && x.IsConfirm == true).ToList();

            var rates = _context.Ratings.ToList();

            movies.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.MovieId == x.Id).Select(s => s.Rate).ToList();
                var movie = _context.Movies.Where(y => y.Id == x.Id && y.IsConfirm == true).FirstOrDefault();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    movie.Avg = (float)avg;
                    movie.AvgCount = tmpRates.Count;
                }

                library.MovieList.Add(movie);
            });

            ViewBag.GenreId = _context.Genres.ToList();
            return View("Films", library);
        }

        public IActionResult SearchSeries(MyLibrary ml)
        {
            var series = _context.Series.Where(x => x.Title.Contains(ml.movieSearch) && x.IsConfirm == true).ToList();

            var rates = _context.Ratings.ToList();

            series.ForEach(x =>
            {
                var tmpRates = rates.Where(y => y.MovieId == x.Id).Select(s => s.Rate).ToList();
                var serie = _context.Series.Where(y => y.Id == x.Id && y.IsConfirm == true).FirstOrDefault();
                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    serie.Avg = (float)avg;
                    serie.AvgCount = tmpRates.Count;
                }

                library.SeriesList.Add(serie);
            });

            ViewBag.GenreId = _context.Genres.ToList();
            return View("Series", library);
        }

        public IActionResult SortByBest()
        {
            int genreId;
            if (TempData["xd"] == null)
            {
                genreId = 0;
            }
            else
            {
                genreId = (int)TempData["xd"];
            }
            TempData.Keep("xd");

            var movies = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            List<MovieSeriesGenre> movieSearch;
            if(genreId != 0)
            {
                movieSearch = _context.MovieSeriesGenres.Where(x => x.GenreId == genreId && x.MovieId != null && x.Movie.IsConfirm == true).ToList();

                movieSearch.ForEach(x =>
                {
                    var movie = _context.Movies.Where(y => y.Id == x.MovieId && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.MovieId == x.MovieId).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        movie.Avg = (float)avg;
                        movie.AvgCount = tmpRates.Count;
                    }
                    library.MovieList.Add(movie);
                });
            }
            else
            {
                movies.ForEach(x =>
                {
                    var movie = _context.Movies.Where(y => y.Id == x.Id && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.MovieId == x.Id).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        movie.Avg = (float)avg;
                        movie.AvgCount = tmpRates.Count;
                    }
                    library.MovieList.Add(movie);
                });
            }

            //library.MovieList = library.MovieList.OrderBy(x => x.Avg).ToList();
            library.MovieList = library.MovieList.OrderByDescending(x => x.Avg).ToList();


            ViewBag.GenreId = _context.Genres.ToList();
            return View("Films", library);

        }

        public IActionResult SortSeriesByBest()
        {
            int genreId;
            if (TempData["xd"] == null)
            {
                genreId = 0;
            }
            else
            {
                genreId = (int)TempData["xd"];
            }
            TempData.Keep("xd");

            var series = _context.Series.Where(x => x.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            List<MovieSeriesGenre> movieSearch;
            if (genreId != 0)
            {
                movieSearch = _context.MovieSeriesGenres.Where(x => x.GenreId == genreId && x.SeriesId != null && x.Series.IsConfirm == true).ToList();

                movieSearch.ForEach(x =>
                {
                    var serie = _context.Series.Where(y => y.Id == x.SeriesId && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.SeriesId == x.SeriesId).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        serie.Avg = (float)avg;
                        serie.AvgCount = tmpRates.Count;
                    }
                    library.SeriesList.Add(serie);
                });
            }
            else
            {
                series.ForEach(x =>
                {
                    var serie = _context.Series.Where(y => y.Id == x.Id && x.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.SeriesId == x.Id).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        serie.Avg = (float)avg;
                        serie.AvgCount = tmpRates.Count;
                    }
                    library.SeriesList.Add(serie);
                });
            }

            //library.MovieList = library.MovieList.OrderBy(x => x.Avg).ToList();
            library.SeriesList = library.SeriesList.OrderByDescending(x => x.Avg).ToList();


            ViewBag.GenreId = _context.Genres.ToList();
            return View("Series", library);

        }

        public IActionResult SortByWorst()
        {
            int genreId;
            if (TempData["xd"] == null)
            {
                genreId = 0;
            }
            else
            {
                genreId = (int)TempData["xd"];
            }
            TempData.Keep("xd");

            var movies = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            List<MovieSeriesGenre> movieSearch;
            if(genreId != 0)
            {
                movieSearch = _context.MovieSeriesGenres.Where(x => x.GenreId == genreId && x.MovieId != null && x.Movie.IsConfirm == true).ToList();

                movieSearch.ForEach(x =>
                {
                    var movie = _context.Movies.Where(y => y.Id == x.MovieId && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.MovieId == x.MovieId).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        movie.Avg = (float)avg;
                        movie.AvgCount = tmpRates.Count;
                    }
                    library.MovieList.Add(movie);
                });
            }
            else
            {
                movies.ForEach(x =>
                {
                    var movie = _context.Movies.Where(y => y.Id == x.Id && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.MovieId == x.Id).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        movie.Avg = (float)avg;
                        movie.AvgCount = tmpRates.Count;
                    }
                    library.MovieList.Add(movie);
                });
            }

            library.MovieList = library.MovieList.OrderBy(x => x.Avg).ToList();

            ViewBag.GenreId = _context.Genres.ToList();
            return View("Films", library);

        }

        public IActionResult SortSeriesByWorst()
        {
            int genreId;
            if (TempData["xd"] == null)
            {
                genreId = 0;
            }
            else
            {
                genreId = (int)TempData["xd"];
            }
            TempData.Keep("xd");

            var series = _context.Series.Where(x => x.IsConfirm == true).ToList();
            var rates = _context.Ratings.ToList();
            List<MovieSeriesGenre> movieSearch;
            if (genreId != 0)
            {
                movieSearch = _context.MovieSeriesGenres.Where(x => x.GenreId == genreId && x.SeriesId != null && x.Series.IsConfirm == true).ToList();

                movieSearch.ForEach(x =>
                {
                    var serie = _context.Series.Where(y => y.Id == x.SeriesId && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.SeriesId == x.SeriesId).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        serie.Avg = (float)avg;
                        serie.AvgCount = tmpRates.Count;
                    }
                    library.SeriesList.Add(serie);
                });
            }
            else
            {
                series.ForEach(x =>
                {
                    var serie = _context.Series.Where(y => y.Id == x.Id && y.IsConfirm == true).FirstOrDefault();
                    var tmpRates = rates.Where(y => y.SeriesId == x.Id).Select(s => s.Rate).ToList();
                    if (tmpRates.Count > 0)
                    {
                        var avg = tmpRates.Average();
                        serie.Avg = (float)avg;
                        serie.AvgCount = tmpRates.Count;
                    }
                    library.SeriesList.Add(serie);
                });
            }

            library.SeriesList = library.SeriesList.OrderBy(x => x.Avg).ToList();

            ViewBag.GenreId = _context.Genres.ToList();
            return View("Series", library);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}