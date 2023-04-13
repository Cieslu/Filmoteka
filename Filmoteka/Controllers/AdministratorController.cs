using Filmoteka.Data;
using Filmoteka.Models;
using Filmoteka.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Filmoteka.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private MyLibrary library = new MyLibrary();

        public AdministratorController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<User> userManager)
        {
            _logger = logger;
            _context = db;
            _userManager = userManager;
        }

        public IActionResult PositionConfirm()
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            library.MovieList = _context.Movies.Where(x => x.IsConfirm == false).ToList();
            library.SeriesList = _context.Series.Where(x => x.IsConfirm == false).ToList();

            library.MovieList.ToList().ForEach(x =>
            {
                var NickName = _userManager.FindByIdAsync(x.UserId).Result.NickName;
                x.NickName =NickName;
            });

            library.SeriesList.ToList().ForEach(x =>
            {
                var NickName = _userManager.FindByIdAsync(x.UserId).Result.NickName;
                x.NickName = NickName;
            });

            ViewBag.Cancel = false;

            return View(library);
        }

        public IActionResult PositionCancel()
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            library.MovieList = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            library.SeriesList = _context.Series.Where(x => x.IsConfirm == true).ToList();

            library.MovieList.ToList().ForEach(x =>
            {
                var NickName = _userManager.FindByIdAsync(x.UserId).Result.NickName;
                x.NickName = NickName;
            });

            library.SeriesList.ToList().ForEach(x =>
            {
                var NickName = _userManager.FindByIdAsync(x.UserId).Result.NickName;
                x.NickName = NickName;
            });

            ViewBag.Cancel = true;

            return View("PositionConfirm", library);
        }

        public async Task<IActionResult> IsConfirmMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            movie.IsConfirm = true;
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PositionConfirm));
        }

        public async Task<IActionResult> IsConfirmSeries(int id)
        {
            var serie = _context.Series.FirstOrDefault(x => x.Id == id);
            serie.IsConfirm = true;
            _context.Update(serie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PositionConfirm));
        }

        public async Task<IActionResult> CancelMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            movie.IsConfirm = false;
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PositionCancel));
        }

        public async Task<IActionResult> CancelSeries(int id)
        {
            var serie = _context.Series.FirstOrDefault(x => x.Id == id);
            serie.IsConfirm = false;
            _context.Update(serie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PositionCancel));
        }

        public IActionResult DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            var movieGenre = _context.MovieSeriesGenres.Where(x => x.MovieId == id).ToList();
            var movieRatings = _context.Ratings.Where(x => x.MovieId == id).ToList();
            var movieComments = _context.Comments.Where(x => x.MovieId == id).ToList();
            var movieLinks = _context.Links.Where(x => x.MovieId == id).ToList();

            movieGenre.ForEach(x =>
            {
                _context.MovieSeriesGenres.Remove(x);
            });

            movieRatings.ForEach(x =>
            {
                _context.Ratings.Remove(x);
            });

            movieComments.ForEach(x =>
            {
                _context.Comments.Remove(x);
            });

            movieLinks.ForEach(x =>
            {
                _context.Links.Remove(x);
            });

            _context.Movies.Remove(movie);

            _context.SaveChanges();

            return RedirectToAction(nameof(PositionConfirm));
        }

        public IActionResult DeleteSeries(int id)
        {
            var serie = _context.Series.FirstOrDefault(x => x.Id == id);
            var serieGenre = _context.MovieSeriesGenres.Where(x => x.SeriesId == id).ToList();
            var serieRatings = _context.Ratings.Where(x => x.SeriesId == id).ToList();
            var serieComments = _context.Comments.Where(x => x.SeriesId == id).ToList();

            var serieSesons = _context.SeriesSesons.Where(x => x.SeriesId == id).ToList();
            //var seson = _context.Sesons.Where(x => x.Id == serieSesons.SesonId).FirstOrDefault();
            var seson = _context.Sesons.ToList();
            var episodes = _context.Episodes.ToList();
            var comments = _context.Comments.ToList();
            var ratings = _context.Ratings.ToList();
            var links = _context.Links.ToList();
            

            serieGenre.ForEach(x =>
            {
                _context.MovieSeriesGenres.Remove(x);
            });

            serieRatings.ForEach(x =>
            {
                _context.Ratings.Remove(x);
            });

            serieComments.ForEach(x =>
            {
                _context.Comments.Remove(x);
            });

            serieSesons.ForEach(x =>
            {
                seson.ForEach(s =>
                {
                    if(x.SesonId == s.Id)
                    {
                        _context.Sesons.Remove(s);
                    }
                });
                
                _context.SeriesSesons.Remove(x);

                episodes.ForEach(y =>
                {
                    comments.ForEach(u =>
                    {
                        if(y.Id == u.EpisodeId)
                        {
                            _context.Comments.Remove(u);
                        }
                    });

                    ratings.ForEach(r =>
                    {
                        if(y.Id == r.EpisodeId)
                        {
                            _context.Ratings.Remove(r);
                        }
                    });

                    links.ForEach(l =>
                    {
                        if (y.Id == l.EpisodeId)
                        {
                            _context.Links.Remove(l);
                        }
                    });

                    if (x.SesonId == y.SesonId)
                    {
                        _context.Episodes.Remove(y);
                    }
                });
            });

            _context.Series.Remove(serie);

            _context.SaveChanges();

            return RedirectToAction(nameof(PositionConfirm));
        }

        public IActionResult ChartIndex()
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

            List<int> MoviesCount = new List<int>();

            MonthsInt.ForEach(x =>
            {
                MoviesCount.Add(_context.Movies.Where(y => y.DateTime.Month == x && y.IsConfirm == true).Count());
            });

            ViewBag.Movies = _context.Movies.Where(x => x.IsConfirm == true).Count();
            ViewBag.MoviesCount = MoviesCount;
            ViewBag.MonthsString = MonthsString;

            return View(library);
        }

        public IActionResult ChartDetails(string id)
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            var movie = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var genres = _context.Genres.ToList();

            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

            List<int> GenresCount = new List<int>();
            List<string> Genres = new List<string>();
            List<string> GenresDistinct = new List<string>();
            List<MovieSeriesGenre> Movies = new List<MovieSeriesGenre>();

            int MonthInt = 0;

            for (int i = 0; i < MonthsString.Count; i++)
            {
                if (MonthsString[i] == id)
                {
                    MonthInt = i + 1;
                }
            }

            /*genres.ForEach(g =>
            {
                var movies = _context.MovieSeriesGenres.Where(x => x.MovieId != null && x.GenreId == g.Id).ToList();
                int counter = 0;

                movies.ForEach(m =>
                {
                    var xd = _context.Movies.Where(x => x.Id == m.MovieId && x.DateTime.Month == MonthInt && x.IsConfirm == true).FirstOrDefault();
                    if(xd != null)
                    {
                        counter++;
                        Genres.Add(m.Genres.Name);
                    }
                });
                GenresCount.Add(counter);
            });

            var genresNew = Genres.Distinct();*/

            genres.ForEach(g =>
            {
                var movies = _context.MovieSeriesGenres.Where(x => x.MovieId != null && x.GenreId == g.Id && x.Movie.DateTime.Month == MonthInt && x.Movie.IsConfirm == true).ToList();

                movies.ForEach(m =>
                {
                    Genres.Add(m.Genres.Name);
                    Movies.Add(m);
                });
            });

            GenresDistinct = Genres.Distinct().ToList();

            GenresDistinct.ForEach(g =>
            {
                GenresCount.Add(Movies.Where(x => x.Genres.Name == g).Count());
            });

            TempData["MonthInt"] = MonthInt;
            TempData["MonthString"] = id;

            ViewBag.genreCount = _context.Movies.Where(x => x.DateTime.Month == MonthInt && x.IsConfirm == true).Count();
            ViewBag.Month = id;
            ViewBag.GenresLabel = GenresDistinct;
            ViewBag.GenresCount = GenresCount;

            return View(library);
        }

        public IActionResult ChartRaiting(string id)
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            List<string> RaitingsName = new List<string> { "Nieporozumienie", "Ujdzie", "Niezły", "Dobry", "Arcydzieło" };
            List<int> Raitings = new List<int>();
            var monthString = TempData["MonthString"];
            TempData.Keep("MonthString");
            var monthInt = TempData["MonthInt"];
            TempData.Keep("MonthInt");

            var movie = _context.Movies.Where(x => x.IsConfirm == true).ToList();
            var genres = _context.Genres.ToList();
            var rate = _context.Ratings.Where(x => x.MovieId != null).ToList();

            var genre = _context.Genres.FirstOrDefault(x => x.Name == id);
            var movies = _context.MovieSeriesGenres.Where(x => x.MovieId != null && x.GenreId == genre.Id).ToList();

            int rate1 = 0;
            int rate2 = 0;
            int rate3 = 0;
            int rate4 = 0;
            int rate5 = 0;

            movies.ForEach(m =>
            {         
                var xd = _context.Movies.Where(x => x.Id == m.MovieId && x.DateTime.Month == (int)monthInt && x.IsConfirm == true).FirstOrDefault();
                if(xd != null && xd.Ratings != null)
                {
                    foreach (var rate in xd.Ratings)
                    {
                        if (rate.Rate == 1)
                        {
                            rate1++;
                        }
                        if (rate.Rate == 2)
                        {
                            rate2++;
                        }
                        if (rate.Rate == 3)
                        {
                            rate3++;
                        }
                        if (rate.Rate == 4)
                        {
                            rate4++;
                        }
                        if (rate.Rate == 5)
                        {
                            rate5++;
                        }
                    }
                }
            });

            Raitings.Add(rate1);
            Raitings.Add(rate2);
            Raitings.Add(rate3);
            Raitings.Add(rate4);
            Raitings.Add(rate5);

            ViewBag.Raitings = RaitingsName;
            ViewBag.RaitingCount = Raitings.Where(x => x != 0).Count();
            ViewBag.monthString = monthString;
            ViewBag.Genre = id;
            ViewBag.RaitingLabel = Raitings;

            return View(library);
        }

        public IActionResult ChartIndexSeries()
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

            List<int> MoviesCount = new List<int>();

            MonthsInt.ForEach(x =>
            {
                MoviesCount.Add(_context.Series.Where(y => y.DateTime.Month == x && y.IsConfirm == true).Count());
            });

            ViewBag.Movies = _context.Series.Where(x => x.IsConfirm == true).Count();
            ViewBag.MoviesCount = MoviesCount;
            ViewBag.MonthsString = MonthsString;

            return View(library);
        }

        public IActionResult ChartDetailsSeries(string id)
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            var movie = _context.Series.Where(x => x.IsConfirm == true).ToList();
            var genres = _context.Genres.ToList();

            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

            List<int> GenresCount = new List<int>();
            List<string> Genres = new List<string>();
            List<string> GenresDistinct = new List<string>();
            List<MovieSeriesGenre> Series = new List<MovieSeriesGenre>();


            int MonthInt = 0;

            for (int i = 0; i < MonthsString.Count; i++)
            {
                if (MonthsString[i] == id)
                {
                    MonthInt = i + 1;
                }
            }

            genres.ForEach(g =>
            {
                var movies = _context.MovieSeriesGenres.Where(x => x.SeriesId != null && x.GenreId == g.Id && x.Series.DateTime.Month == MonthInt && x.Series.IsConfirm == true).ToList();

                movies.ForEach(m =>
                {
                    Genres.Add(m.Genres.Name);
                    Series.Add(m);
                });
            });

            GenresDistinct = Genres.Distinct().ToList();

            GenresDistinct.ForEach(g =>
            {
                GenresCount.Add(Series.Where(x => x.Genres.Name == g).Count());
            });

            TempData["MonthInt"] = MonthInt;
            TempData["MonthString"] = id;

            ViewBag.genreCount = _context.Series.Where(x => x.DateTime.Month == MonthInt && x.IsConfirm == true).Count();
            ViewBag.Month = id;
            ViewBag.GenresLabel = GenresDistinct;
            ViewBag.GenresCount = GenresCount;

            return View(library);
        }

        public IActionResult ChartRaitingSeries(string id)
        {
            var UserId = _userManager.GetUserId(User);
            library.MoviesML = _context.Movies.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();
            library.SeriesML = _context.Series.Where(x => x.UserId == UserId && x.IsConfirm == true).ToList();

            List<string> RaitingsName = new List<string> { "Nieporozumienie", "Ujdzie", "Niezły", "Dobry", "Arcydzieło" };
            List<int> Raitings = new List<int>();
            var monthString = TempData["MonthString"];
            TempData.Keep("MonthString");
            var monthInt = TempData["MonthInt"];
            TempData.Keep("MonthInt");

            var movie = _context.Series.Where(x => x.IsConfirm == true).ToList();
            var genres = _context.Genres.ToList();
            var rate = _context.Ratings.Where(x => x.SeriesId != null).ToList();

            var genre = _context.Genres.FirstOrDefault(x => x.Name == id);
            var movies = _context.MovieSeriesGenres.Where(x => x.SeriesId != null && x.GenreId == genre.Id).ToList();

            int rate1 = 0;
            int rate2 = 0;
            int rate3 = 0;
            int rate4 = 0;
            int rate5 = 0;

            movies.ForEach(m =>
            {
                var xd = _context.Series.Where(x => x.Id == m.SeriesId && x.DateTime.Month == (int)monthInt && x.IsConfirm == true).FirstOrDefault();
                if(xd != null && xd.Ratings != null)
                {
                    foreach (var rate in xd.Ratings)
                    {
                        if (rate.Rate == 1)
                        {
                            rate1++;
                        }
                        if (rate.Rate == 2)
                        {
                            rate2++;
                        }
                        if (rate.Rate == 3)
                        {
                            rate3++;
                        }
                        if (rate.Rate == 4)
                        {
                            rate4++;
                        }
                        if (rate.Rate == 5)
                        {
                            rate5++;
                        }
                    }
                }
            });

            Raitings.Add(rate1);
            Raitings.Add(rate2);
            Raitings.Add(rate3);
            Raitings.Add(rate4);
            Raitings.Add(rate5);

            ViewBag.Raitings = RaitingsName;
            ViewBag.RaitingCount = Raitings.Where(x => x != 0).Count();
            ViewBag.monthString = monthString;
            ViewBag.Genre = id;
            ViewBag.RaitingLabel = Raitings;

            return View(library);
        }
    }
}
