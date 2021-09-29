using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };

            var customers = new List<Customer>();
            customers.Add(new Customer { Name = "Customer 1" });
            customers.Add(new Customer { Name = "Customer 2" });

            RandomMovieViewModel viewModel = new RandomMovieViewModel();
            viewModel.Movie = movie;
            viewModel.Customers = customers;

            return View(viewModel);

            //my notes
            //return Content("Hello world!");
            //return new EmptyResult();
            //return Redirect("url");
            //return Json();
            //return File("file");
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" });

            //ViewData["Movie"] = movie;
            //return View();
        }

        [Route("movies/released/{year}/{month}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            ParamsViewModel pvm = new ParamsViewModel();
            pvm.year = year;
            pvm.month = month;
            return View("Params", pvm);
            //return Content(year + "/" + month);
        }

        public ActionResult Edit(int id)
        {
            return Content("id=" + id);
        }

        // movies
        //public ActionResult Index(int? pageIndex, string sortBy)
        //{
        //    // int? - means it can be entered as null
        //    // so if doesn't 'havevalue' set it to 1.
        //    if (!pageIndex.HasValue) { pageIndex = 1; }
        //    if (String.IsNullOrWhiteSpace(sortBy)) sortBy = "Name";

        //    return Content("pageIndex=" + pageIndex + "&sortBy=" + sortBy);
        //}

    }
}