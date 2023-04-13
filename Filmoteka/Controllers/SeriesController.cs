using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Filmoteka.Data;
using Filmoteka.Models;
using Microsoft.AspNetCore.Identity;
using Filmoteka.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using TMDbLib.Client;

namespace Filmoteka.Controllers
{
    public class SeriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public SeriesController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Series.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            TempData["id"] = id;
            var UserId = _userManager.GetUserId(User);
            ViewBag.UserId = UserId;
            var series = _context.Series.FirstOrDefault(m => m.Id == id);
            var seriesGenreComment = new MovieGenreCommentsViewModel();

            seriesGenreComment.Series = series;

            var seriesGenre = _context.MovieSeriesGenres.Where(x => x.SeriesId == id).ToList();

            seriesGenre.ForEach(mg =>
            {
                seriesGenreComment.Genres.Add(_context.Genres.Where(g => g.Id == mg.GenreId).FirstOrDefault());
            });

            var seriesComment = _context.Comments.Where(x => x.SeriesId == id).ToList();

            seriesGenreComment.Comments = seriesComment;

            var sesons = _context.SeriesSesons.Where(x => x.SeriesId == id).ToList();

            sesons.ForEach(s =>
            {
                seriesGenreComment.Sesons.Add(_context.Sesons.Where(x => x.Id == s.SesonId).FirstOrDefault());
            });

            if (series == null)
            {
                return NotFound();
            }

            var ifRated = _context.Ratings.Where(x => x.UserId == UserId && x.SeriesId == id).FirstOrDefault();

            if (ifRated != null)
            {
                TempData["Rate"] = ifRated.Rate;
            }

            //pobieranie ocen z bazy
            var movieRateCount = _context.Ratings.Where(x => x.SeriesId == id).ToList().Count;
            if (movieRateCount > 0)
            {
                series.Avg = (float)_context.Ratings.Where(x => x.SeriesId == id).Select(s => s.Rate).Average();
            }
            series.AvgCount = movieRateCount;

            //oceny z api
            TMDbClient client = new TMDbClient("07ccb68af3750babdf2c2b47d75369c6");
            var responseSerie2 = client.SearchTvShowAsync(series.Title).Result.Results;

            if(responseSerie2.Count != 0)
            {
                var responseSerie = client.SearchTvShowAsync(series.Title).Result.Results[0];

                series.TMDBRate = (float)responseSerie.VoteAverage;
                series.TMDBRateCount = responseSerie.VoteCount;
                series.TMDBlink = $"https://www.themoviedb.org/tv/{responseSerie.Id}";

            } else {
                ViewBag.TMDBError = true;
            }


            seriesGenreComment.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            seriesGenreComment.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();

            return View(seriesGenreComment);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");

            var movieGenreComment = new MovieGenreCommentsViewModel();
            var UserId = _userManager.GetUserId(User);

            movieGenreComment.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            movieGenreComment.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();
            return View(movieGenreComment);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Director,RelaseDate,Description,Video,MovieSeriesGenreId,Image")] Series series)
        {
            var UserId = _userManager.GetUserId(User);

            if (series.Image != null)
            {
                if (!series.Video.Contains("https://www.youtube.com/watch?v") || series.Video.Length < 43 || series.Video.Length > 46)
                {
                    ViewData["ErrorVideo"] = "Link do filmu jest nieprawidłowy";
                    ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
                    return View(series);
                }
                if (ModelState.IsValid)
                {
                    series.UserId = UserId;
                    series.Photo = "";
                    series.Video = convertLinkToVideo(series.Video);
                    _context.Add(series);
                    await _context.SaveChangesAsync();

                    UploadSeriesPhoto(series);
                    if(series.Image == null)
                    {
                        _context.Remove(series);
                        _context.SaveChanges();

                        var tmpMovie = new MovieGenreCommentsViewModel();
                        tmpMovie.Series = series;

                        ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
                        return View(tmpMovie);
                    }
                    series.Photo = $"/img/series/{series.Id}/index.jpg";
                    _context.Series.Update(series);

                    series.MovieSeriesGenreId.ToList().ForEach(x =>
                    {
                        var NewSeries = new MovieSeriesGenre()
                        {
                            SeriesId = series.Id,
                            GenreId = Convert.ToInt32(x)
                        };
                        _context.Add(NewSeries);
                    });
                    await _context.SaveChangesAsync();

                    var seson = new Seson
                    {
                        SesonCounter = 1,
                        Name = "Sezon 1"
                    };
                    _context.Add(seson);
                    await _context.SaveChangesAsync();

                    var sesonId = _context.Sesons.OrderBy(x => x.Id).LastOrDefault();

                    var seriesSeson = new SeriesSeson
                    {
                        SeriesId = series.Id,
                        SesonId = sesonId.Id
                    };
                    _context.Add(seriesSeson);
                    await _context.SaveChangesAsync();

                    return Redirect("/Home/Series");
                }
            }
            else
            {
                ViewData["ErrorImage"] = "Pole Dodaj zdjęcie jest wymagane";
            }

            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
            return View(series);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int? id, [Bind("Description")] Comment comment)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            TempData["id"] = id;
            var user = await _userManager.GetUserAsync(User);


            comment.Name = user.NickName;
            comment.SeriesId = id;
            comment.DateTime = DateTime.Now;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCommentEpisode(int? id, [Bind("Description")] Comment comment)
        {
            if (id == null || _context.Episodes == null)
            {
                return NotFound();
            }

            TempData["id"] = id;
            var user = await _userManager.GetUserAsync(User);


            comment.Name = user.NickName;
            comment.EpisodeId = id;
            comment.DateTime = DateTime.Now;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("EpisodeDetails", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLinkToMovie(int? id, [Bind("Name", "Quality", "URL")] Link link)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            link.Username = user.NickName;
            link.EpisodeId = id;

            _context.Add(link);
            await _context.SaveChangesAsync();
            return RedirectToAction("EpisodeDetails", new { id });
        }

        [Authorize]
        public async Task<IActionResult> CreateSeson(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var seriesSesons = _context.SeriesSesons.Where(x => x.SeriesId == id);
            var counter = _context.SeriesSesons.OrderBy(x => x.Id).LastOrDefault();
            var counterLast = _context.Sesons.FirstOrDefault(x => x.Id == counter.SesonId);

            var seson = new Seson
            {
                SesonCounter = counterLast.SesonCounter + 1,
                Name = "Sezon " + Convert.ToInt32(counterLast.SesonCounter + 1)
            };
            _context.Add(seson);
            await _context.SaveChangesAsync();

            var sesonId = _context.Sesons.OrderBy(x => x.Id).LastOrDefault();

            var seriesSeson = new SeriesSeson
            {
                SeriesId = Convert.ToInt32(id),
                SesonId = sesonId.Id
            };
            _context.Add(seriesSeson);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRate(MovieGenreCommentsViewModel test, int id)
        {
            var rate = new Rating();
            rate.SeriesId = id;
            rate.Rate = test.Rate;
            rate.UserId = _userManager.GetUserId(User);

            _context.Add(rate);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEpisodeRate(MovieGenreCommentsViewModel test, int id)
        {
            var rate = new Rating();
            rate.EpisodeId = id;
            rate.Rate = test.Rate;
            rate.UserId = _userManager.GetUserId(User);

            _context.Add(rate);
            _context.SaveChanges();

            return RedirectToAction("EpisodeDetails", new { id });
        }

        public IActionResult SesonDetails(int? id)
        {
            var episode = new EpisodeVievModel();
            var UserId = _userManager.GetUserId(User);
            ViewBag.UserId = UserId;
            TempData["id"] = id;
            List<Episode> episodes = _context.Episodes.Where(x => x.SesonId == id).ToList();
            var seriesId = _context.SeriesSesons.Where(x => x.SesonId == id).Select(s => s.SeriesId).FirstOrDefault();
            string video = _context.Series.Where(w => w.Id == seriesId).Select(s => s.Video).FirstOrDefault();
            ViewBag.video = video;

            var ratings = _context.Ratings.ToList();
            var comments = _context.Comments.ToList();

            episodes.ForEach(x =>
            {
                var tmpRates = ratings.Where(y => y.EpisodeId == x.Id).Select(s => s.Rate).ToList();
                var tmpComments = comments.Where(y => y.EpisodeId == x.Id).Count();

                if (tmpRates.Count > 0)
                {
                    var avg = tmpRates.Average();
                    x.Avg = (float)avg;
                    x.AvgCount = tmpRates.Count;
                }

                if (tmpComments > 0)
                {
                    x.CommentCount = tmpComments;
                }
            });

            episode.EpisodeList = episodes;
            episode.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            episode.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();

            return View(episode);
        }

        public IActionResult CreateEpisode(int? id)
        {
            TempData["id"] = id;

            var episode = new EpisodeVievModel();
            var UserId = _userManager.GetUserId(User);

            episode.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            episode.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();
            return View(episode);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode([Bind("Title,Date,Description,Image")] Episode episode, int id)
        {
            if (episode.Image != null)
            {
                if (ModelState.IsValid)
                {
                    episode.Photo = "";
                    episode.SesonId = id;
                    _context.Add(episode);
                    await _context.SaveChangesAsync();

                    var Episodes = _context.Episodes.Where(x => x.SesonId == id).ToList().Count();

                    var SeriesId = _context.SeriesSesons.Where(x => x.SesonId == id).Select(x => x.SeriesId).FirstOrDefault();
                    UploadEpisodePhoto(episode, SeriesId, Episodes);
                    if (episode.Image == null)
                    {
                        ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
                        return View(episode);
                    }
                    episode.Photo = $"/img/series/{SeriesId}/episode{Episodes}.jpg";

                    _context.Episodes.Update(episode);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("SesonDetails", new {id});
                }
            }
            else
            {
                ViewData["ErrorImage"] = "Pole Dodaj zdjęcie jest wymagane";
            }

            return View(episode);
        }

        public IActionResult EpisodeDetails(int id)
        {
            var sesonId = _context.Episodes.Where(x => x.Id == id).Select(x => x.SesonId).FirstOrDefault();
            TempData["SesonId"] = sesonId;
            var Episode = new EpisodeVievModel();
            Episode.Episode = _context.Episodes.Where(x => x.Id == id).FirstOrDefault();

            var UserId = _userManager.GetUserId(User);

            var ifRated = _context.Ratings.Where(x => x.UserId == UserId && x.EpisodeId == id).FirstOrDefault();

            if (ifRated != null)
            {
                TempData["Rate"] = ifRated.Rate;
            }   

            Episode.Comments = _context.Comments.Where(x => x.EpisodeId == id).ToList();

            //pobieranie ocen z bazy
            var movieRateCount = _context.Ratings.Where(x => x.EpisodeId == id).ToList().Count;
            if (movieRateCount > 0)
            {
                Episode.Avg = (float)_context.Ratings.Where(x => x.EpisodeId == id).Select(s => s.Rate).Average();
            }
            Episode.AvgCount = movieRateCount;

            Episode.Links = _context.Links.Where(x => x.EpisodeId == id).ToList();

            Episode.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            Episode.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();

            return View(Episode);
        }
































        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var series = await _context.Series.FindAsync(id);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Director,RelaseDate,Description,Photo,Video,MovieSeriesGenreId")] Series series)
        {
            if (id != series.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(series);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriesExists(series.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(series);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Series == null)
            {
                return NotFound();
            }

            var series = await _context.Series
                .FirstOrDefaultAsync(m => m.Id == id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Series == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Series'  is null.");
            }
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                _context.Series.Remove(series);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriesExists(int id)
        {
          return _context.Series.Any(e => e.Id == id);
        }

        private string convertLinkToVideo(string url)
        {
            string videoID = url.Substring(url.Length - 11);

            return $"https://www.youtube.com/embed/{videoID}?autoplay=1&controls=0&rel=0&modestbranding=1&mute=1&loop=1";
        }

        private void UploadSeriesPhoto(Series series)
        {
            string path = Path.Combine(_environment.WebRootPath, "img", "series", series.Id.ToString());
            Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(series.Image.FileName);
            if (!fileInfo.Extension.ToLower().Equals(".jpg") && !fileInfo.Extension.ToLower().Equals(".jpeg") && !fileInfo.Extension.ToLower().Equals(".png"))
            {
                ModelState.AddModelError("IncorrectExtensionError", "Nie prawidłowy format pliku - akceptowalne rozszerzenia to: png/jpg/jpeg");
                series.Image = null;
            }
            else
            {
                string fileName = "index.jpg";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    series.Image.CopyTo(stream);
                }

            }

        }

        private void UploadEpisodePhoto(Episode episode, int SeriesId, int episodeNumber)
        {
            string path = Path.Combine(_environment.WebRootPath, "img", "series", SeriesId.ToString());
            Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(episode.Image.FileName);
            if (!fileInfo.Extension.ToLower().Equals(".jpg") && !fileInfo.Extension.ToLower().Equals(".jpeg") && !fileInfo.Extension.ToLower().Equals(".png"))
            {
                ModelState.AddModelError("IncorrectExtensionError", "Nie prawidłowy format pliku - akceptowalne rozszerzenia to: png/jpg/jpeg");
                episode.Image = null;
            }
            else
            {
                string fileName = $"episode{episodeNumber}.jpg";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    episode.Image.CopyTo(stream);
                }

            }

        }
    }
}
