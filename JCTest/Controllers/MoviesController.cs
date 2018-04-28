using JCTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JCTest.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMoviesModel moviesModel;

        public MoviesController(
            IMoviesModel moviesModel)
        {
            this.moviesModel = moviesModel;
        }

        /// <summary>
        /// Show list of movies.
        /// Get: Movies
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.movies = this.moviesModel.GetList();
            return View();
        }

        /// <summary>
        /// Show details of the selected movie.
        /// Get: Movies/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
