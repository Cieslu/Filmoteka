using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Filmoteka.Data;
using Filmoteka.Models;
using Filmoteka.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using TMDbLib.Client;

namespace Filmoteka.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;

        public MoviesController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
              return View(_context.Movies.ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            TempData["id"] = id;
            var UserId = _userManager.GetUserId(User);
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            var movieGenreComment = new MovieGenreCommentsViewModel();

            movieGenreComment.Movie = movie;

            var movieGenre = _context.MovieSeriesGenres.Where(x => x.MovieId == id).ToList();

            movieGenre.ForEach(mg =>
            {
                movieGenreComment.Genres.Add(_context.Genres.Where(g => g.Id == mg.GenreId).FirstOrDefault());
            });

            var movieComment = _context.Comments.Where(x => x.MovieId == id).ToList();

            movieGenreComment.Comments = movieComment;

            var ifRated = _context.Ratings.Where(x => x.UserId == UserId && x.MovieId == id).FirstOrDefault();

            if (ifRated != null)
            {
                TempData["Rate"] = ifRated.Rate; 
            }

            //pobieranie ocen z bazy
            var movieRateCount = _context.Ratings.Where(x => x.MovieId == id).ToList().Count;
            if (movieRateCount > 0)
            {
                movie.Avg = (float)_context.Ratings.Where(x => x.MovieId == id).Select(s => s.Rate).Average();
            }
            movie.AvgCount = movieRateCount;

            //oceny z api
            TMDbClient client = new TMDbClient("07ccb68af3750babdf2c2b47d75369c6");
            var responseMovie2 = client.SearchMovieAsync(movie.Title).Result.Results;
            if (responseMovie2.Count != 0)
            {
                var responseMovie = client.SearchMovieAsync(movie.Title).Result.Results[0];
                movie.TMDBRate = (float)responseMovie.VoteAverage;
                movie.TMDBRateCount = responseMovie.VoteCount;
                movie.TMDBlink = $"https://www.themoviedb.org/movie/{responseMovie.Id}";

            } else {
                ViewBag.TMDBError = true;
            }

            movieGenreComment.Links = _context.Links.Where(x => x.MovieId == id).ToList();
            movieGenreComment.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            movieGenreComment.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();

            if (movie == null)
            {
                return NotFound();
            }

            return View(movieGenreComment);
        }

        [Authorize]
        public IActionResult Create()
        {
            var movieGenreComment = new MovieGenreCommentsViewModel();
            var UserId = _userManager.GetUserId(User);

            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
            movieGenreComment.MoviesML = _context.Movies.Where(x => x.UserId == UserId).ToList();
            movieGenreComment.SeriesML = _context.Series.Where(x => x.UserId == UserId).ToList();
            return View(movieGenreComment);
        }
 
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Director,RelaseDate,Description,Video,MovieSeriesGenreId,Image")] Movie movie)
        {
            var UserId = _userManager.GetUserId(User);

            if (movie.Image != null)
            { 
                if (!movie.Video.Contains("https://www.youtube.com/watch?v") || movie.Video.Length < 43 || movie.Video.Length > 46)
                {
                    ViewData["ErrorVideo"] = "Link do filmu jest nieprawidłowy";
                    ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
                    return View(movie);
                }
                if (ModelState.IsValid)
                {
                    movie.UserId = UserId;
                    movie.Photo = "";
                    movie.Video = convertLinkToVideo(movie.Video);
                    _context.Add(movie);
                    await _context.SaveChangesAsync();

                    UploadMainPhoto(movie);
                    if (movie.Image == null)
                    {
                        _context.Remove(movie);
                        _context.SaveChanges();

                        var tmpMovie = new MovieGenreCommentsViewModel();
                        tmpMovie.Movie = movie;

                        ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
                        return View(tmpMovie);
                    }
                    movie.Photo = $"/img/films/{movie.Id}.jpg";
                    _context.Movies.Update(movie);

                    movie.MovieSeriesGenreId.ToList().ForEach(x =>
                    {
                        var NewMovie = new MovieSeriesGenre()
                        {
                            MovieId = movie.Id,
                            GenreId = Convert.ToInt32(x)
                        };
                        _context.Add(NewMovie);
                    });
                    await _context.SaveChangesAsync();

                    return Redirect("/Home/Films");
                }
            }
            else
            {
                ViewData["ErrorImage"] = "Pole Dodaj zdjęcie jest wymagane";
            }
          
            ViewBag.GenreId = new SelectList(_context.Genres, "Id", "Name");
            return View(movie);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRate(MovieGenreCommentsViewModel test, int id)
        {
            var rate = new Rating();
            rate.MovieId = id;
            rate.Rate = test.Rate;
            rate.UserId = _userManager.GetUserId(User);

            _context.Add(rate);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(int? id, [Bind("Description")] Comment comment)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            TempData["id"] = id;
            var user = await _userManager.GetUserAsync(User);

            
            
            comment.Name = user.NickName;
            comment.MovieId = id;
            comment.DateTime = DateTime.Now;
            _context.Add(comment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddLinkToMovie(int? id, [Bind("Name","Quality","URL")] Link link)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            link.Username = user.NickName;
            link.MovieId = id;

            _context.Add(link);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });

            /*  if (link.URL.Contains("vidoza") || link.URL.Contains("upstream"))
              {


                      _context.Add(link);
                      await _context.SaveChangesAsync();
                      return RedirectToAction("Details", new { id });

              } 
              else 
              {
                  return RedirectToAction("Details", new { id });
              }*/

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Director,RelaseDate,Description,Photo,Video,MovieSeriesGenreId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return _context.Movies.Any(e => e.Id == id);
        }

        private string convertLinkToVideo(string url)
        {
            string videoID = url.Substring(url.Length - 11);

            return $"https://www.youtube.com/embed/{videoID}?autoplay=1&controls=0&rel=0&modestbranding=1&mute=1&loop=1";
        }

        private void UploadMainPhoto(Movie movie)
        {
            string path = Path.Combine(_environment.WebRootPath, "img", "films");
            Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(movie.Image.FileName);
            if (!fileInfo.Extension.ToLower().Equals(".jpg") && !fileInfo.Extension.ToLower().Equals(".jpeg") && !fileInfo.Extension.ToLower().Equals(".png"))
            {
                ModelState.AddModelError("IncorrectExtensionError", "Nie prawidłowy format pliku - akceptowalne rozszerzenia to: png/jpg/jpeg");
                movie.Image = null;
            }
            else
            {
                string fileName = movie.Id + ".jpg";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    movie.Image.CopyTo(stream);
                }

            }

        }
    }
}


