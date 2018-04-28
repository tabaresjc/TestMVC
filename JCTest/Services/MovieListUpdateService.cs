using JCTest.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace JCTest.Services
{
    public class MovieListUpdateService : IMovieListUpdateService
    {
        private readonly NLog.ILogger logger;

        private readonly ISettingsService settings;

        public MovieListUpdateService(
            NLog.ILogger logger,
            ISettingsService settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        /// <summary>
        /// Fetch the latest movies given by a minimum date
        /// </summary>
        /// <param name="language">string, Specify a language to query translatable fields with.</param>
        /// <param name="minimumDate">Datetime object, Filter and only include movies that have a release date</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public void FetchAndSave(string language, DateTime minimumDate, Hangfire.IJobCancellationToken cancellationToken)
        {
            using (var client = new ServiceClient(this.settings.Get("TMDBApiKey")))
            {
                var task = Task.Run(async () =>
                {
                    return await client.Movies.DiscoverAsync(
                        language,
                        true,
                        null,
                        minimumDate,
                        null,
                        null,
                        null,
                        null,
                        null,
                        1,
                        cancellationToken.ShutdownToken);
                });

                var movies = task.Result;

                foreach (var movie in movies.Results)
                {
                    this.logger.Info("{0} - {1}", movie.Title, movie.OriginalTitle);
                }
            }
        }


    }
}