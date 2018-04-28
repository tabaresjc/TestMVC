using JCTest.Interfaces;
using JCTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JCTest.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMoviesModel moviesModel;

        private readonly ISettingsService settings;

        private readonly NLog.ILogger logger;

        public MoviesController(
            IMoviesModel moviesModel,
            NLog.ILogger logger,
            ISettingsService settings)
        {
            this.moviesModel = moviesModel;
            this.logger = logger;
            this.settings = settings;
        }

        /// <summary>
        /// Show list of movies.
        /// Get: Movies
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string search = "")
        {
            ViewBag.movies = this.moviesModel.GetList(search);
            ViewBag.keyword = search.Trim();
            return View();
        }

        /// <summary>
        /// Show details of the selected movie.
        /// Get: Movies/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var movieInfo = ViewBag.movieInfo = this.moviesModel.Get(id);

            if (movieInfo == null)
            {
                return new HttpStatusCodeResult(404);
            }

            var apiKey = this.settings.Get("TMDBApiKey");

            var client = new ServiceClient(apiKey);

            try
            {
                var m = ViewBag.movieData = await client.Movies.GetAsync(
                    movieInfo.MovieId,
                    movieInfo.Language,
                    true,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error while retrieving movie data [{0}] from TMDB.",
                    movieInfo.MovieId);
                return new HttpStatusCodeResult(500);
            }
            finally
            {
                client?.Dispose();
            }

            return View();
        }
    }
}
