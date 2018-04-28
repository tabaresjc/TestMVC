using JCTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JCTest.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly IMovieListUpdateService movieListUpdateService;

        public HomeController(
            IMovieListUpdateService movieListUpdateService)
        {
            this.movieListUpdateService = movieListUpdateService;
        }

        public ActionResult Index(CancellationToken cancellationToken)
        {
            Hangfire.BackgroundJob.Enqueue(() => LongRunningMethod(Hangfire.JobCancellationToken.Null));

            return View();
        }

        public void LongRunningMethod(Hangfire.IJobCancellationToken cancellationToken)
        {
            this.movieListUpdateService.FetchAndSave("en-US", new DateTime(DateTime.UtcNow.Year, 1, 1), cancellationToken);
        }
    }
}