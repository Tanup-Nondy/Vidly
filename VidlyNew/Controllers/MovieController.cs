using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModel;
using VidlyNew.ViewModel;

namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        /*public ActionResult Random()
        {
            var movie = new Movie() { Name = "Fast and Furious!" };
            var customers = new List<Customer>
            {
                new Customer{Name = "Customer1"},
                new Customer{Name ="Customer2"}
            };
            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);
        }
        /*public ActionResult Edit(int id)
        {
            return Content("id=" + id);
        }
        public ActionResult Index(int?IndexPage,string sortby)
        {
            if (!IndexPage.HasValue)
                IndexPage = 1;
            if (String.IsNullOrWhiteSpace(sortby))
                sortby = "name";
            return Content(string.Format("Pageindex {0} and sort by {1}", IndexPage, sortby));
        }*/
        /* [Route("movie/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
         public ActionResult ByReleaseDate(int year,int month)
         {

             return Content(year+"/"+month);
         }   */
        private VidContext _context;

        public MovieController()
        {
            _context=new VidContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ViewResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
            if (movie == null)
                return HttpNotFound();
            return View(movie);
        }
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }
        public ViewResult New()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new NewMovieFormModel()
            {
                Genres = genres
            };

            return View("NewMovieAdd", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMovieSave(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewMovieFormModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };
                return View("NewMovieAdd", viewModel);
            }
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.ReleaseDate = movie.ReleaseDate;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Movie");
        }
        public ActionResult EditMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);
            if (movie == null)
                HttpNotFound();
            var viewModel = new NewMovieFormModel(movie)
            {
                Genres = _context.Genres.ToList()
            };
            return View("NewMovieAdd", viewModel);
        }

        
    }

}