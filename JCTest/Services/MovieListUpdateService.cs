using JCTest.Interfaces;
using JCTest.Models.Interfaces;
using JCTest.Models.Movies;
using System;
using System.Linq;
using System.Net.TMDb;
using System.Threading.Tasks;

namespace JCTest.Services
{
    public class MovieListUpdateService : IMovieListUpdateService
    {
        private readonly IMoviesModel moviesModel;

        private readonly NLog.ILogger logger;

        private readonly ISettingsService settings;

        public const string ApplicationSettingMovieMinDate = "ApplicationSettingMovieMinDate";

        public MovieListUpdateService(
            IMoviesModel moviesModel,
            NLog.ILogger logger,
            ISettingsService settings)
        {
            this.moviesModel = moviesModel;
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
        public void FetchAndSave(int year, Hangfire.IJobCancellationToken cancellationToken)
        {
            var language = this.settings.Get("TMDBApiLanguage", "en-US");
            var apiKey = this.settings.Get("TMDBApiKey");
            var count = 0; var maxPages = 5; var page = 1;

            var client = new ServiceClient(apiKey);

            try
            {
                this.logger.Info("Start scrapping movies via discovery...");

                while (page < maxPages)
                {
                    var task = Task.Run(async () =>
                    {
                        return await client.Movies.DiscoverAsync(
                            language,
                            true,
                            year,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            page,
                            cancellationToken.ShutdownToken);
                    });

                    var data = task.Result;

                    if (data.Results == null || data.Results.Count() <= 0)
                    {
                        break;
                    }

                    if (maxPages != data.PageCount)
                    {
                        maxPages = data.PageCount;
                        this.logger.Info("Updated maxpages {0}.", data.PageCount);
                    }

                    var movies = data.Results
                        .Select(m => MovieInfo.ConvertFrom(m, language))
                        .ToList();

                    this.moviesModel.AddOrUpdate(movies);

                    count += movies.Count;
                    page += 1;

                    this.logger.Info("Processing page {0}/{1} from api resource. ",
                        page,
                        data.PageCount);
                }

                this.logger.Info("Processed {0} movie(s) into the database.", count);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error while saving movies data in database");
                throw ex;
            }
            finally
            {
                if(client!=null)
                {
                    client.Dispose();
                }
            }
        }
    }
}