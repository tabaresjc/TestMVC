using JCTest.Interfaces;
using JCTest.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JCTest.Models.Movies
{
    public class MoviesModel : IMoviesModel
    {
        private readonly ApplicationDbContext dbContext;

        private readonly ISettingsService settings;

        public MoviesModel(
            ApplicationDbContext dbContext,
            ISettingsService settings)
        {
            this.dbContext = dbContext;
            this.settings = settings;
        }

        public int UpdateCount { get; set; }

        public int CreatedCount { get; set; }

        public MovieInfo Get(int id)
        {
            return this.dbContext.Movies.Find(id);
        }

        public IList<MovieInfo> GetList(
            string keywords = null, 
            int page= 1, 
            int limit = 24, 
            string orderby = "popularity_desc")
        {
            var q = this.dbContext.Movies
                .AsEnumerable()
                .Where(x => !x.Adult && x.ReleaseDate != null);

            // filter by keywords
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                keywords = keywords.ToLower();
                q = q.Where(x => x.Title.ToLower().Contains(keywords) || x.OriginalTitle.ToLower().Contains(keywords));
            }

            switch (orderby)
            {
                case "popularity_asc":
                    q = q.OrderBy(x => x.Popularity).AsEnumerable();
                    break;
                case "popularity_desc":
                    q = q.OrderByDescending(x => x.Popularity).AsEnumerable();
                    break;
                case "release_date_asc":
                    q = q.OrderBy(x => x.ReleaseDate).AsEnumerable();
                    break;
                case "release_date_desc":
                default:
                    q = q.OrderByDescending(x => x.ReleaseDate).AsEnumerable();
                    break;
            }

            return q.Skip((page - 1) * limit).Take(limit).ToList();
        }

        public void AddOrUpdate(IList<MovieInfo> movies)
        {
            var dbContextTransaction = this.dbContext.Database.BeginTransaction();

            try
            {
                this.UpdateCount = this.CreatedCount = 0;

                movies.ToList()
                    .ForEach(x =>
                    {
                        var existingMovie = this.dbContext.Movies
                            .Where(m => m.MovieId == x.MovieId)
                            .FirstOrDefault();

                        if (existingMovie == null)
                        {
                            this.dbContext.Movies.Add(x);
                            this.CreatedCount += 1;
                        }
                        else
                        {
                            x.Id = existingMovie.Id;
                            var entry = this.dbContext.Entry(existingMovie);
                            entry.CurrentValues.SetValues(x);

                            this.UpdateCount += 1;
                        }
                    });

                this.dbContext.SaveChanges();
                dbContextTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
            }
        }
    }
}